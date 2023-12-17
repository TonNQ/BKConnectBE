using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Files
{
    public interface IFileRepository
    {
        Task<List<UploadedFile>> GetListFilesOfClassRoomAsync(long roomId);
        Task<UploadedFile> GetFileByIdAsync(long fileId);
        Task AddFileAsync(UploadedFile file);
    }
}