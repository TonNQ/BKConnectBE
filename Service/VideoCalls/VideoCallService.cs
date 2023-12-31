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

        public async Task<List<ParticipantInfo>> GetParticipantInfosInCall(long roomId)
        {
            var videoCall = StaticParams.VideoCallList.FirstOrDefault(x => x.RoomId == roomId);

            if (videoCall == null)
            {
                return new();
            }

            var listParticipant = videoCall.Participants.ToDictionary(x => x.UserId, x => x.PeerId);
            var users = await _userRepository
                .GetListUsersByListIdAsync(listParticipant.Select(x => x.Key).ToList());
            var listParticipantInfo = _mapper.Map<List<ParticipantInfo>>(users);

            foreach (var participantInfo in listParticipantInfo)
            {
                participantInfo.PeerId = listParticipant[participantInfo.Id];
            }
            return listParticipantInfo;
        }

        public async Task<List<ParticipantInfo>> GetUserInfoWhenJoinCall(long roomId, string userId)
        {
            var videoCall = StaticParams.VideoCallList
                .FirstOrDefault(x => x.RoomId == roomId && x.Participants.Any(p => p.UserId == userId));

            if (videoCall == null)
            {
                return new();
            }

            var participant = videoCall.Participants.FirstOrDefault(p => p.UserId == userId);
            var user = await _userRepository.GetUserByIdAsync(participant.UserId);

            var participantInfo = _mapper.Map<ParticipantInfo>(user);
            participantInfo.PeerId = participant.PeerId;
            return new List<ParticipantInfo>() { participantInfo };
        }
    }
}