using BKConnectBE.Model.Dtos.UploadedFileManagement;

namespace BKConnectBE.Service.Files
{
    public interface IFileService
    {
        Task<List<UploadedFileDto>> GetListFilesOfClassRoomAsync(long roomId);
        Task<long> AddFileAsync(string userId, AddFileDto file);
    }
}