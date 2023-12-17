using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.UploadedFileManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Files;
using BKConnectBE.Repository.Rooms;
using BKConnectBE.Repository.Users;

namespace BKConnectBE.Service.Files
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGenericRepository<UploadedFile> _genericRepositoryForUploadedFile;
        private readonly IMapper _mapper;

        public FileService(IFileRepository fileRepository,
            IUserRepository userRepository,
            IRoomRepository roomRepository,
            IGenericRepository<UploadedFile> genericRepositoryForUploadedFile,
            IMapper mapper)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _genericRepositoryForUploadedFile = genericRepositoryForUploadedFile;
            _mapper = mapper;
        }

        public async Task<List<UploadedFileDto>> GetListFilesOfClassRoomAsync(long roomId)
        {
            var list = await _fileRepository.GetListFilesOfClassRoomAsync(roomId);
            return _mapper.Map<List<UploadedFileDto>>(list);
        }

        public async Task<long> AddFileAsync(string userId, AddFileDto file)
        {
            if (!await _userRepository.IsLecturer(userId) || !await _roomRepository.IsInRoomAsync(file.RoomId, userId))
            {
                throw new Exception(MsgNo.ERROR_UP_FILE);
            }
            var uploadedFile = new UploadedFile
            {
                Path = file.Path,
                RoomId = file.RoomId,
                UserId = userId,
                UploadTime = DateTime.UtcNow.AddHours(7)
            };
            await _fileRepository.AddFileAsync(uploadedFile);

            await _genericRepositoryForUploadedFile.SaveChangeAsync();
            return uploadedFile.Id;
        }
    }
}