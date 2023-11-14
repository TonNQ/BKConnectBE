using System.ComponentModel;

namespace BKConnectBE.Common.Enumeration
{
    public enum NotificationType
    {
        [Description("lời mời kết bạn")]
        IsSendFriendRequest,

        [Description("chấp nhận lời mời kết bạn")]
        IsAcceptFriendRequest,

        [Description("xóa khỏi nhóm")]
        IsOutRoom,

        [Description("giảng viên gửi file")]
        IsPostFile,
    }
}