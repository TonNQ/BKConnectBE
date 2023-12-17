using BKConnectBE.Model.Dtos.ClassFileManagement;

namespace BKConnectBE.Service.Files
{
    public interface IFileService
    {
        Task<List<ClassFileDto>> GetListFilesOfClassRoomAsync(long roomId);
        Task<long> AddFileAsync(string userId, AddFileDto file);
    }
}