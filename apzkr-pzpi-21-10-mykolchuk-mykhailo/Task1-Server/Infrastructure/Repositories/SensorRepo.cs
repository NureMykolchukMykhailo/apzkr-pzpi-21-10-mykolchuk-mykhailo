using Microsoft.EntityFrameworkCore;

namespace APZ_backend.Repositories
{
    public class SensorRepo
    {
        DatabaseContext context;
        public SensorRepo(DatabaseContext _context)
        {
            context = _context;
        }

        public async Task<ICollection<Sensor>> GetSensorsAsync()
        {
            return await context.Sensors.Include(sensor => sensor.Car)
                .ThenInclude(car => car.Drivers).ToListAsync();
        }

        public async Task<Sensor?> GetSensorAsync(int id)
        {
            return await context.Sensors.Include(s => s.Car)
                .ThenInclude(car => car.Drivers).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Sensor>> GetSensorsByUserAsync(int userId)
        {
            return await context.Sensors.Where(s => s.OwnerId == userId).Include(sensor => sensor.Car)
                .ThenInclude(car => car.Drivers).ToListAsync();
        }

        public async Task<bool> InsertSensorAsync(Sensor sensor)
        {
            await context.Sensors.AddAsync(sensor);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSensorAsync(int id, string name)
        {
            Sensor? sensor = await context.Sensors.FindAsync(id);
            if (sensor is not null)
                sensor.Name = name;
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSensorAsync(int id)
        {
            context.Sensors.Remove(await GetSensorAsync(id));
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ConnectSensorToCar(int carId, int sensorId)
        {
            Car? car = await context.Cars.FindAsync(carId);
            Sensor? sensor = await context.Sensors.FindAsync(sensorId);
            if(car is not null && sensor is not null)
            {
                car.SensorId = sensorId;
                sensor.CarId = carId;
            }
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DisconnectSensorFromCar(int carId)
        {
            Car? car = await context.Cars.Where(car => car.Id == carId)
                .Include(car => car.Sensor).FirstOrDefaultAsync();

            if (car is not null && car.Sensor is not null)
            {
                car.Sensor.CarId = null;
                car.SensorId = null;
            }
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<Sensor>> GetFreeSensorsByUserId(int userId)
        {
            return await context.Sensors
                .Where(sensor => sensor.OwnerId == userId && sensor.CarId == null).ToListAsync();
        }
    }
}
