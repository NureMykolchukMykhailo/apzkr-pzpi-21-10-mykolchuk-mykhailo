using Microsoft.EntityFrameworkCore;

namespace APZ_backend.Repositories
{
    public class CarRepo
    {
        DatabaseContext context;
        public CarRepo(DatabaseContext _context)
        {
            context = _context;
        }

        public async Task<ICollection<Car>> GetCarsAsync()
        {
            return await context.Cars.Include(car => car.Sensor)
                .Include(car => car.Drivers).ToListAsync();
        }

        public async Task<Car?> GetCarAsync(int id)
        {
            return await context.Cars.Where(car => car.Id == id).Include(car => car.Sensor)
                .Include(car => car.Drivers).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Car>> GetCarsByUserAsync(int ownerId)
        {
            return await context.Cars.Where(car => car.OwnerId == ownerId).Include(car => car.Sensor)
                .Include(car => car.Drivers).ToListAsync();
        }


        public async Task<bool> InsertCarAsync(Car car)
        {
            await context.Cars.AddAsync(car);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            context.Cars.Update(car);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            context.Cars.Remove(await GetCarAsync(id));
            return await context.SaveChangesAsync() > 0;
        }

    }
}
