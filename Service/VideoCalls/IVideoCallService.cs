using BKConnectBE.Model.Dtos.VideoCallManagement;

namespace BKConnectBE.Service.VideoCalls
{
    public interface IVideoCallService
    {
        Task<List<ParticipantInfo>> GetParticipantInfos(long roomId);
    }
}