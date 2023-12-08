using System.ComponentModel;

namespace BKConnectBE.Common.Enumeration
{
    public enum SystemMessageType
    {
        IsBecomeFriend,

        [Description("thêm vào nhóm")]
        IsInRoom,

        [Description("xoá khỏi nhóm")]
        IsOutRoom,

        [Description("rời khỏi nhóm")]
        IsLeaveRoom,

        [Description("tạo nhóm")]
        IsCreateGroupRoom,

        IsStartCall,

        IsJoinCall,

        IsLeaveCall,

        IsEndCall,
    }
}