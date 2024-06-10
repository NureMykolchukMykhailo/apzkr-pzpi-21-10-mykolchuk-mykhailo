using Microsoft.EntityFrameworkCore;

namespace APZ_backend.Repositories
{
    public class UserRepo
    {
        DatabaseContext context;
        public UserRepo(DatabaseContext _context)
        {
            context = _context;
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            User? user = await context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
            if(user is not null)
            {
                bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (isPasswordCorrect)
                    return user;
            }
            return null;
        }

        public async Task<bool> InsertUserAsync(User user)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(8);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
            user.Password = hashedPassword;
            await context.Users.AddAsync(user);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            User? u = await context.Users.FindAsync(id);
            string salt = BCrypt.Net.BCrypt.GenerateSalt(8);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
            
            if (u is not null)
            {
                u.Name = user.Name;
                u.Password = hashedPassword;
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            context.Users.Remove(await GetUserAsync(id));
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<string> ChangeLanguage(int id, string lang)
        {
            User? user = await context.Users.FindAsync(id);
            if(user is not null)
            {
                user.Language = lang;
                //context.Users.Update(user);
                await context.SaveChangesAsync();
                return lang;
            }
            
            return "fail";
        }
    }
}
