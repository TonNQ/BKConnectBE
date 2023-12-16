using BKConnectBE.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BKConnectBE.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BKConnectContext _context;

        private readonly DbSet<T> _db;

        public GenericRepository(BKConnectContext context)
        {
            this._context = context;
            _db = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _db.AddAsync(entity);
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _db.ToListAsync();
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _db.FindAsync(id);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _db.FindAsync(id);
        }

        public async Task RemoveByIdAsync(long id)
        {
            var entity = await _db.FindAsync(id);
            if (entity is not null)
            {
                _db.Remove(entity);
            }
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        public async Task SaveChangeAsync()
        {
            IDbContextTransaction transaction = null;
            try
            {
                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction?.RollbackAsync();
                throw;
            }
        }
    }
}