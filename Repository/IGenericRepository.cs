using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository;

public interface IGenericRepository <T> where T : BaseEntity
{
    void Add(T entity);

    T GetById(int id);

    void Save();
}
