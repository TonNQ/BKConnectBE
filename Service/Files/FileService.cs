using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.ClassFileManagement;
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
        private readonly IGenericRepository<ClassFile> _genericRepositoryForClassFile;
        private readonly IMapper _mapper;

        public FileService(IFileRepository fileRepository,
            IUserRepository userRepository,
            IRoomRepository roomRepository,
            IGenericRepository<ClassFile> genericRepositoryForClassFile,
            IMapper mapper)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _genericRepositoryForClassFile = genericRepositoryForClassFile;
            _mapper = mapper;
        }

        public async Task<List<ClassFileDto>> GetListFilesOfClassRoomAsync(long roomId)
        {
            var list = await _fileRepository.GetListFilesOfClassRoomAsync(roomId);
            return _mapper.Map<List<ClassFileDto>>(list);
        }

        public async Task<long> AddFileAsync(string userId, AddFileDto file)
        {
            if (!await _userRepository.IsLecturer(userId) || !await _roomRepository.IsInRoomAsync(file.RoomId, userId))
            {
                throw new Exception(MsgNo.ERROR_UP_FILE);
            }
            var classFile = new ClassFile
            {
                Content = file.Content,
                RoomId = file.RoomId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow.AddHours(7)
            };
            await _fileRepository.AddFileAsync(classFile);

            await _genericRepositoryForClassFile.SaveChangeAsync();
            return classFile.Id;
        }
    }
}