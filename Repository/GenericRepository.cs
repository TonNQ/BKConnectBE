using BKConnectBE.Model;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository;

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

    public async Task<T> GetByIdAsync(int id)
    {
        return await _db.FindAsync(id);
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _db.FindAsync(id);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
