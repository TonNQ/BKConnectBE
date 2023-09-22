using ChatFriend.Model;
using ChatFriend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatFriend.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private BKConnectContext _context;

    private DbSet<T> _db;

    public GenericRepository(BKConnectContext context)
    {
        this._context = context;
        _db = context.Set<T>();
    }

    public void Add(T entity)
    {
        entity.UpdatedDate = DateTime.Now;
        entity.CreatedDate = DateTime.Now;
        _db.Add(entity);
    }

    public T GetById(int id)
    {
        return _db.Find(id);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
