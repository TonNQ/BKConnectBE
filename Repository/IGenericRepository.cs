namespace BKConnectBE.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync(string id);

        Task SaveChangeAsync();
    }
}