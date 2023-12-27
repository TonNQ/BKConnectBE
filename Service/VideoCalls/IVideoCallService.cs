using BKConnectBE.Model.Dtos.VideoCallManagement;

namespace BKConnectBE.Service.VideoCalls
{
    public interface IVideoCallService
    {
        Task<List<ParticipantInfo>> GetParticipantInfosInCall(long roomId);
        Task<List<ParticipantInfo>> GetUserInfoWhenJoinCall(long roomId, string userId);
    }
}