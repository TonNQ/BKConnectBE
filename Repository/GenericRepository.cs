using BKConnectBE.Model;
using BKConnectBE.Model.Entities;
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

    public void Add(T entity)
    {
        _db.Add(entity);
    }

    public T GetById(int id)
    {
        return _db.Find(id);
    }

    public T GetById (string id)
    {
        return _db.Find(id);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
