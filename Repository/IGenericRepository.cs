namespace BKConnectBE.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task<T> GetByIdAsync(long id);

        Task<T> GetByIdAsync(string id);

        Task RemoveByIdAsync(long id);

        Task SaveChangeAsync();
    }
}
