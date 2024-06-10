using Microsoft.EntityFrameworkCore;


namespace APZ_backend.Repositories
{
    public class RecordRepo
    {
        DatabaseContext context;
        public RecordRepo(DatabaseContext _context)
        {
            context = _context;
        }

        public async Task<ICollection<Record>> GetRecordsAsync()
        {
            return await context.Records.Include(r => r.EngineSpeeds)
                .Include(r => r.SuddenBraking).ToListAsync();
        }

        public async Task<Record?> GetRecordAsync(int id)
        {
            return await context.Records.Where(record => record.Id == id).Include(r => r.EngineSpeeds)
                .Include(r => r.SuddenBraking).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Record>> GetRecordsByCarAsync(int carId)
        {
            return await context.Records.Where(r => r.CarId == carId).Include(r => r.EngineSpeeds)
                .Include(r => r.SuddenBraking).ToListAsync();
        }


        public async Task<bool> InsertRecordAsync(Record record)
        {
            await context.Records.AddAsync(record);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRecordAsync(Record record)
        {
            context.Records.Update(record);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRecordAsync(int id)
        {
            context.Records.Remove(await GetRecordAsync(id));
            return await context.SaveChangesAsync() > 0;
        }

    }
}
