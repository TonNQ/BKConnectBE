using BKConnectBE.Model.Dtos.VideoCallManagement;

namespace BKConnectBE.Service.VideoCalls
{
    public interface IVideoCallService
    {
        Task<List<ParticipantInfo>> GetParticipantInfosWithoutUser(long roomId, string userId);
        Task<List<ParticipantInfo>> GetUserInfoWhenJoinCall(long roomId, string userId);
    }
}