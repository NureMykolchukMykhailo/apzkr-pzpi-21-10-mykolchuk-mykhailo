using System.Text.Json;
using Dumpify;
using Microsoft.EntityFrameworkCore;

namespace APZ_backend.Infrastructure.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;

    public class KafkaConsumerService : BackgroundService
    {
        IServiceProvider services;
        public KafkaConsumerService(IServiceProvider _provider)
        {
            services = _provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "my-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            List<State> states = new();

            using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
            {
                consumer.Subscribe("apz");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);
                        string message = result.Message.Value;

                        if (message == "Finished")
                        {
                            var statesCopy = new List<State>(states);
                            Thread producerThread = new(() =>
                            {
                                Handle(statesCopy);
                            });

                            producerThread.Start();
                            states.Clear();
                            Console.WriteLine(states.Count());
                            continue;
                        }

                        states.Add(JsonSerializer.Deserialize<State>(result.Message.Value));
                        Console.WriteLine("added");
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }

        }

        private void Handle(List<State> states)
        {
            Console.WriteLine("Hello from another thread");
            Console.WriteLine(states.Count);

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                Car? car = context.Cars.Where(car => car.SensorId == states[1].DeviceId).FirstOrDefault();

                if (car is not null)
                {
                    (int leftTurns, int rightTurns) = TurnsLeftAndRight(states);
                    (int dangerousLeftTurns, int dangerousRightTurns) = DangerousTurnsLeftAndRirht(states);
                    (List<EngineSpeedMoment> low, List<EngineSpeedMoment> high) = LowAndHighEngineSpeeds(states);
                    int fastStart = FastStart(states);
                    List<BrakingMoment> suddenBraking = SuddenBraking(states);

                    Record record = new()
                    {
                        Car = car,
                        TripStart = states.First().EngineStarted,
                        TripEnd = states.Last().Time,
                        LeftTurns = leftTurns,
                        RightTurns = rightTurns,
                        DangerousLeftTurns = dangerousLeftTurns,
                        DangerousRightTurns = dangerousRightTurns,
                        EngineSpeeds = low.Concat(high).ToList(),
                        FastStart = fastStart,
                        SuddenBraking = suddenBraking
                    };

                    record.Dump();

                    context.Records.Add(record);
                    context.SaveChanges();
                }
            }

        }

        private (int, int) TurnsLeftAndRight(List<State> states)
        {
            int left = 0, right = 0;

            for (int i = 0; i < states.Count - 1; i++)
            {
                if (states[i].SteeringWheelAngle == 0 && states[i + 1].SteeringWheelAngle < 0)
                    left++;

                if (states[i].SteeringWheelAngle == 0 && states[i + 1].SteeringWheelAngle > 0)
                    right++;
            }
            return (left, right);
        }

        private (int, int) DangerousTurnsLeftAndRirht(List<State> states)
        {
            int left = 0, right = 0;

            int statesToSkip = 0;

            for (int i = 0; i < states.Count; i++)
            {
                if (statesToSkip > 0)
                {
                    statesToSkip--;
                    continue;
                }

                if (states[i].SteeringWheelAngle == 0)
                    continue;

                if (states[i].Speed > 10 && !states[i].ReturnWheelToBase)
                {
                    try
                    {
                        State current = states[i];
                        State nextSecondState = states[i + 5];
                        // поворот вліво
                        if (current.SteeringWheelAngle < 0)
                        {
                            //якщо протягом наступної секунди різкий поворот керма більше ніж на 50 градусів
                            //при великій швидкості - небезпечний поворот
                            if (nextSecondState.SteeringWheelAngle - current.SteeringWheelAngle < -50)
                            {
                                left++;
                                statesToSkip = 5;
                                continue;
                            }
                        }
                        //поворот вправо
                        else if (current.SteeringWheelAngle > 0)
                        {
                            if (nextSecondState.SteeringWheelAngle - current.SteeringWheelAngle > 50)
                            {
                                right++;
                                statesToSkip = 5;
                                continue;
                            }
                        }

                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return (left, right);
        }

        private (List<EngineSpeedMoment>, List<EngineSpeedMoment>) LowAndHighEngineSpeeds(List<State> states)
        {
            List<EngineSpeedMoment> low = new();
            List<EngineSpeedMoment> high = new();

            LinkedList<State> linkedStates = new(states);

            var current = linkedStates.First;
            while (current is not null)
            {
                if (current.Value.EngineSpeed > 1000 && current.Value.EngineSpeed < 5000)
                {
                    current = current.Next;
                    continue;
                }

                if (current.Value.EngineSpeed <= 1000)
                {
                    EngineSpeedMoment lowMoment = new()
                    {
                        Begin = current.Value.Time
                    };

                    List<int> engineSpeeds = new();
                    while (current is not null && current.Value.EngineSpeed <= 1000)
                    {
                        engineSpeeds.Add(current.Value.EngineSpeed);
                        lowMoment.End = current.Value.Time;
                        current = current.Next;
                    }
                    lowMoment.AvgEngineSpeed = engineSpeeds.Average();

                    if (lowMoment.End - lowMoment.Begin > TimeSpan.FromSeconds(1))
                        low.Add(lowMoment);

                    if (current is null)
                        break;
                }

                if (current.Value.EngineSpeed >= 5000)
                {
                    EngineSpeedMoment highMoment = new()
                    {
                        Begin = current.Value.Time
                    };

                    List<int> engineSpeeds = new();
                    while (current is not null && current.Value.EngineSpeed >= 5000)
                    {
                        engineSpeeds.Add(current.Value.EngineSpeed);
                        highMoment.End = current.Value.Time;
                        current = current.Next;
                    }
                    highMoment.AvgEngineSpeed = engineSpeeds.Average();

                    if (highMoment.End - highMoment.Begin > TimeSpan.FromSeconds(1))
                        low.Add(highMoment);

                    if (current is null)
                        break;
                }
            }

            return (low, high);

        }

        private int FastStart(List<State> states)
        {
            return states[1].ConsiderFastStart;
        }

        private List<BrakingMoment> SuddenBraking(List<State> states)
        {
            List<BrakingMoment> suddenBraking = new();
            LinkedList<State> linkedStates = new(states);

            var current = linkedStates.First;
            var next = current.Next;
            while (next is not null)
            {
                if (current.Value.Speed > 20 && next.Value.Speed < current.Value.Speed / 1.5)
                {
                    suddenBraking.Add(new BrakingMoment()
                    {
                        Time = current.Value.Time,
                        InitialSpeed = current.Value.Speed,
                        SubsequentSpeed = next.Value.Speed
                    });
                }
                current = current.Next;
                next = next.Next;
            }

            return suddenBraking;
        }
    }

}
