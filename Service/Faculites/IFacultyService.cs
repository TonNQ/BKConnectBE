using BKConnectBE.Model.Dtos.FacultyManagement;

namespace BKConnectBE.Service.Faculites
{
    public interface IFacultyService
    {
        Task<FacultyDto> GetFacultyByIdAsync(string facultyId);
        Task<List<FacultyDto>> GetAllFaculiesAsync();
    }
}