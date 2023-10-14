using BKConnectBE.Model.Dtos.ClassManagement;

namespace BKConnectBE.Service.Classes
{
    public interface IClassService
    {
        Task<ClassDto> GetClassByIdAsync(long classId);

        Task<List<ClassDto>> GetAllClassesAsync();
    }
}