using AutoMapper;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Repository.Files;

namespace BKConnectBE.Service.Files
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;

        public FileService(IFileRepository fileRepository,
            IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<List<FileDto>> GetListFilesOfClassRoomAsync(long roomId)
        {
            var list = await _fileRepository.GetListFilesOfClassRoomAsync(roomId);
            return _mapper.Map<List<FileDto>>(list);
        }
    }
}