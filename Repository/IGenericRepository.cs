using ChatFriend.Model.Entities;

namespace ChatFriend.Repository;

public interface IGenericRepository <T> where T : BaseEntity
{
    void Add(T entity);

    T GetById(int id);

    void Save();
}
