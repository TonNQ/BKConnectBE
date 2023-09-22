using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository;

public interface IGenericRepository <T> where T : class
{
    void Add(T entity);

    T GetById(int id);

    T GetById(string id);

    void Save();
}
