using AutoMapper;
using BKConnectBE.Common;
using BKConnectBE.Model.Dtos.VideoCallManagement;
using BKConnectBE.Repository.Users;

namespace BKConnectBE.Service.VideoCalls
{
    public class VideoCallService : IVideoCallService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public VideoCallService(IMapper mapper,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<ParticipantInfo>> GetParticipantInfos(long roomId)
        {
            var videoCall = StaticParams.VideoCallList.FirstOrDefault(x => x.RoomId == roomId);

            if (videoCall == null)
            {
                return new();
            }

            var users = await _userRepository.GetListUsersByListIdAsync(videoCall.UserIds);
            return _mapper.Map<List<ParticipantInfo>>(users);
        }
    }
}