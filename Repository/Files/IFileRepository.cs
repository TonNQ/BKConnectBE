using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Files
{
    public interface IFileRepository
    {
        Task<List<ClassFile>> GetListFilesOfClassRoomAsync(long roomId);
    }
}