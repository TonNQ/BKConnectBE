using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Files
{
    public interface IFileRepository
    {
        Task<List<ClassFile>> GetListFilesOfClassRoomAsync(long roomId);
        Task<ClassFile> GetFileByIdAsync(long fileId);
        Task AddFileAsync(ClassFile file);
    }
}