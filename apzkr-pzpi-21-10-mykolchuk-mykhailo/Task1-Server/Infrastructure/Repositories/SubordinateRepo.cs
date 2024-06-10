using Microsoft.EntityFrameworkCore;

namespace APZ_backend.Repositories
{
    public class SubordinateRepo
    {
        DatabaseContext context;
        public SubordinateRepo(DatabaseContext _context)
        {
            context = _context;
        }

        public async Task<ICollection<Subordinate>> GetSubordinatesAsync()
        {
            return await context.Subordinates.Include(s => s.Car)
                .ThenInclude(c => c.Sensor).ToListAsync();
        }

        public async Task<Subordinate?> GetSubordinateAsync(int id)
        {
            return await context.Subordinates.Where(s => s.Id == id).Include(s => s.Car)
                .ThenInclude(c => c.Sensor).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Subordinate>> GetSubordinatesByUserAsync(int userId)
        {
            return await context.Subordinates.Where(s => s.ChiefId == userId).Include(s => s.Car)
                .ThenInclude(c => c.Sensor).ToListAsync();
        }


        public async Task<bool> InsertSubordinateAsync(Subordinate subordinate)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(8);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(subordinate.Password, salt);
            subordinate.Password = hashedPassword;
            await context.Subordinates.AddAsync(subordinate);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSubordinateAsync(int id, string name)
        {
            Subordinate? subordinate = await context.Subordinates.FindAsync(id);
            if (subordinate is not null)
            {
                subordinate.Name = name;
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteSubordinateAsync(int id)
        {
            context.Subordinates.Remove(await GetSubordinateAsync(id));
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<Subordinate?> GetSubordinateByEmailAndPasswordAsync(string email, string password)
        {
            Subordinate? subordinate = await context.Subordinates.Where(s => s.Email == email).FirstOrDefaultAsync();
            if (subordinate is not null)
            {
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, subordinate.Password);
                if (isPasswordCorrect)
                    return subordinate;
            }
            return null;
        }

        public async Task<bool> ConnectSubordinateToCar(int id, int carId)
        {
            Subordinate? subordinate = await context.Subordinates.FindAsync(id);
            if (subordinate is not null)
            {
                subordinate.CarId = carId;
            }
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DisconnectSubordinateFromCar(int id)
        {
            Subordinate? subordinate = await context.Subordinates.FindAsync(id);
            if (subordinate is not null)
            {
                subordinate.CarId = null;
            }
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<string> ChangeLanguage(int id, string lang)
        {
            Subordinate? subordinate = await context.Subordinates.FindAsync(id);
            if(subordinate is not null)
            {
                subordinate.Language = lang;
                await context.SaveChangesAsync();
                return lang;
            }

            //context.Subordinates.Update(subordinate);
            return "fail";
        }
    }
}
