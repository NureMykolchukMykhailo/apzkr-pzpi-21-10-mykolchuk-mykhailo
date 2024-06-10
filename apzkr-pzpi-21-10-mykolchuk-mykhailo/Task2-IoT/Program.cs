using Dumpify;
using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using APZ_IoT.Models;
using APZ_IoT.Settings;
using APZ_IoT.Settings.User;
using APZ_IoT.Settings.Admin;

namespace APZ_IoT
{
    internal class Program
    {
        public const int TimeInterval = 200; //m/s
        private static Random random;

        static int DeviceId;

        static int MaxEngineSpeed;
        static int MinEngineSpeed;

        static int MaxAcceleration;
        static int MinAcceleration;
        static double MaxSpeed; // m/s

        static string KafkaServer;
        static string KafkaTopic;

        static void InitializeSettings()
        {
            UserSettings uSettings = AppSettingsHelper.GetUserSettings();
            AdminSettings aSettings = AppSettingsHelper.GetAdminSettings();

            DeviceId = Convert.ToInt32(uSettings.DeviceId);
            MaxAcceleration = Convert.ToInt32(uSettings.MaxAcceleration);
            MinAcceleration = Convert.ToInt32(uSettings.MinAcceleration);
            MaxSpeed = Convert.ToDouble(uSettings.MaxSpeed);
            MaxEngineSpeed = Convert.ToInt32(uSettings.MaxEngineSpeed);
            MinEngineSpeed = Convert.ToInt32(uSettings.MinEngineSpeed);

            KafkaServer = aSettings.BootstrapServers;
            KafkaTopic = aSettings.KafkaTopic;
        }

        static async Task Main(string[] args)
        {
            await AppSettingsHelper.StartSettingsDialog();

            InitializeSettings();

            var config = new ProducerConfig
            {
                BootstrapServers = KafkaServer,
            };

            random = new Random();

            Stack<State> states = new();

            states.Push(new State()
            {
                DeviceId = DeviceId,
                SteeringWheelAngle = 0,
                SteeringWheelCount = 0,
                ReturnWheelToBase = false,
                Time = DateTime.Now,
                EngineStarted = DateTime.Now,
                EngineSpeed = MinEngineSpeed
            });

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                while (!Console.KeyAvailable)
                {
                    State newState = GenerateStateV2(states.Pop());
                    states.Push(newState);

                    newState.Dump();
                    producer.Produce(KafkaTopic, new Message<Null, string> { Value = JsonSerializer.Serialize(newState) });

                    Thread.Sleep(TimeInterval);
                }
                producer.Produce(KafkaTopic, new Message<Null, string> { Value = "Finished" });
            }

        }

        private static State GenerateStateV2(State oldState)
        {
            var newState = SimulateWheelTurn(oldState);

            // Рух вперед
            if (oldState.SteeringWheelAngle == 0)
            {
                newState.Acceleration = (random.NextDouble() < 0.6 && oldState.Acceleration < MaxAcceleration)
                    || oldState.Acceleration < MinAcceleration
                    ? oldState.Acceleration + random.Next(0, 2)
                    : oldState.Acceleration - random.Next(1, 3);

                newState.EngineSpeed = oldState.EngineSpeed + newState.Acceleration * 20;

                newState.EngineSpeed = newState.EngineSpeed < MinEngineSpeed ? MinEngineSpeed : newState.EngineSpeed;
                newState.EngineSpeed = newState.EngineSpeed > MaxEngineSpeed ? MaxEngineSpeed : newState.EngineSpeed;

                newState = CalculateSpeed(newState, oldState);
                return newState;
            } // Починаємо поворот ліворуч
            if (newState.SteeringWheelAngle < oldState.SteeringWheelAngle && newState.SteeringWheelAngle < 0)
            {
                newState.Acceleration = 0 - Convert.ToInt32(oldState.Acceleration * 0.7);
                newState.EngineSpeed = oldState.EngineSpeed + newState.Acceleration * 20;
                newState = CalculateSpeed(newState, oldState);
                return newState;
            } // Повертаємо кермо в звичайне положення після повороту ліворуч
            if (newState.SteeringWheelAngle > oldState.SteeringWheelAngle && newState.SteeringWheelAngle < 0)
            {
                newState.Acceleration = oldState.Acceleration + random.Next(0, 2);
                newState.EngineSpeed = oldState.EngineSpeed + newState.Acceleration * 20;
                newState = CalculateSpeed(newState, oldState);
                return newState;
            } // Починаємо поворот праворуч
            if (newState.SteeringWheelAngle > oldState.SteeringWheelAngle && newState.SteeringWheelAngle > 0)
            {
                newState.Acceleration = 0 - Convert.ToInt32(oldState.Acceleration * 0.7);
                newState.EngineSpeed = oldState.EngineSpeed + newState.Acceleration * 20;
                newState = CalculateSpeed(newState, oldState);
                return newState;
            } // Повертаємо кермо в звичайне положення після повороту праворуч
            if (newState.SteeringWheelAngle < oldState.SteeringWheelAngle && newState.SteeringWheelAngle > 0)
            {
                newState.Acceleration = oldState.Acceleration + random.Next(0, 2);
                newState.EngineSpeed = oldState.EngineSpeed + newState.Acceleration * 20;
                newState = CalculateSpeed(newState, oldState);
                return newState;
            }
            return newState;
        }

        public static State CalculateSpeed(State newState, State oldState)
        {
            if (oldState.Speed > MaxSpeed)
            {
                // Симуляція різкого гальмування
                newState.Speed = oldState.Speed * 0.5;
                newState.EngineSpeed = oldState.EngineSpeed / 2;
                newState.Time = DateTime.Now;
                return newState;
            }

            if (oldState.Acceleration > newState.Acceleration)
            {
                newState.Speed = oldState.Speed
                   - ((double)newState.Acceleration * 0.4);
            }
            else
            {
                newState.Speed = oldState.Speed
                    + ((double)newState.Acceleration * 0.2);
            }
            newState.Time = DateTime.Now;
            return newState;
        }

        public static State SimulateWheelTurn(State _previousState)
        {
            State newState = new()
            {
                DeviceId = _previousState.DeviceId,
                EngineStarted = _previousState.EngineStarted,
                SteeringWheelAngle = 0,
                SteeringWheelCount = 0,
                ReturnWheelToBase = false,
                Acceleration = _previousState.Acceleration,
                Speed = _previousState.Speed,
                EngineSpeed = _previousState.EngineSpeed,
            };

            // Вважатимемо що кут у 90 градусів людина повертає десь за 2 секунди
            // У нас інтервал 0,2, тобто в 1 секунді 5 інтервалів
            // тому крок у 9 градусів виглядає оптимальним

            // Рух вперед
            if (_previousState.SteeringWheelAngle == 0)
            {
                newState.SteeringWheelAngle = random.NextDouble() < 0.05 ? random.Next(-9, 10) : 0;
                return newState;
            } // Поворот ліворуч
            else if (_previousState.SteeringWheelAngle < 0)
            {
                if (_previousState.ReturnWheelToBase)
                {
                    // 6 тут гарне значення. Виходить 6 * 4 = 24 градуси в секунду ми повертаємося в початкове положення
                    newState.SteeringWheelAngle = _previousState.SteeringWheelAngle + 6;
                    // якщо вийшли за межі в позитивний кут, то встановлюємо в 0
                    newState.SteeringWheelAngle = newState.SteeringWheelAngle > 0 ? 0 : newState.SteeringWheelAngle;
                    newState.ReturnWheelToBase = true;
                    return newState;
                }

                if (_previousState.SteeringWheelCount >= 10)
                {
                    // Якщо повернули кермо на 90 градусів, то 50/50 повернутися назад або крутити далі
                    if (random.NextDouble() < 0.5) 
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle - 9;
                        newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                        return newState;
                    }
                    else
                    {
                        // Залишаємо як є
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle;
                        newState.ReturnWheelToBase = true;
                        return newState;
                    }
                }
                if (_previousState.SteeringWheelCount >= 20)
                {
                    // Якщо повернули кермо на 180 градусів то 25% крутити далі, інакше повернутися назад
                    if (random.NextDouble() < 0.25) 
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle - 9;
                        newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                        return newState;
                    }
                    else
                    {
                        // Залишаємо як є
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle;
                        newState.ReturnWheelToBase = true;
                        return newState;
                    }
                }
                if (_previousState.SteeringWheelCount >= 30)
                {
                    // Якщо повернули кермо на 270 градусів то 10% крутити далі, інакше повернутися назад
                    if (random.NextDouble() < 0.10) 
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle - 9;
                        newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                        return newState;
                    }
                    else
                    {
                        // Залишаємо як є
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle;
                        newState.ReturnWheelToBase = true;
                        return newState;
                    }
                }

                newState.SteeringWheelAngle = random.NextDouble() < 0.9 ? _previousState.SteeringWheelAngle - 9 : _previousState.SteeringWheelAngle;
                newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                return newState;
            } // Поворот праворуч
            else
            {
                if (_previousState.ReturnWheelToBase)
                {
                    newState.SteeringWheelAngle = _previousState.SteeringWheelAngle - 6;
                    newState.SteeringWheelAngle = newState.SteeringWheelAngle < 0 ? 0 : newState.SteeringWheelAngle; 
                    newState.ReturnWheelToBase = true;
                    return newState;
                }
                if (_previousState.SteeringWheelCount >= 10)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle + 9;
                        newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                        return newState;
                    }
                    else
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle;
                        newState.ReturnWheelToBase = true;
                        return newState;
                    }
                }
                if (_previousState.SteeringWheelCount >= 20)
                {
                    if (random.NextDouble() < 0.25)
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle + 9;
                        newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                        return newState;
                    }
                    else
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle;
                        newState.ReturnWheelToBase = true;
                        return newState;
                    }
                }
                if (_previousState.SteeringWheelCount >= 30)
                {
                    if (random.NextDouble() < 0.10)
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle + 9;
                        newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                        return newState;
                    }
                    else
                    {
                        newState.SteeringWheelAngle = _previousState.SteeringWheelAngle;
                        newState.ReturnWheelToBase = true;
                        return newState;
                    }
                }

                newState.SteeringWheelAngle = random.NextDouble() < 0.9 ? _previousState.SteeringWheelAngle + 9 : _previousState.SteeringWheelAngle;
                newState.SteeringWheelCount = _previousState.SteeringWheelCount + 1;
                return newState;
            }
        }
    }
}