using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Classes
{
    public interface IClassRepository
    {
        Task<Class> GetClassByIdAsync(long classId);

        Task<List<Class>> GetAllClassesAsync();
    }
}