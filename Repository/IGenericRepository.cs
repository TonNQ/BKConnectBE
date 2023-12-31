﻿namespace BKConnectBE.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        
        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(long id);

        Task<T> GetByIdAsync(string id);

        Task RemoveByIdAsync(long id);
        
        void Remove(T entity);

        Task SaveChangeAsync();
    }
}
