using BKConnectBE.Model.Dtos.MessageManagement;

namespace BKConnectBE.Service.Files
{
    public interface IFileService
    {
        Task<List<FileDto>> GetListFilesOfClassRoomAsync(long roomId);
    }
}