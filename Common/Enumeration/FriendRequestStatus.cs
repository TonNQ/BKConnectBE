using System.ComponentModel;

namespace BKConnectBE.Common.Enumeration
{
    public enum FriendRequestStatus
    {
        [Description("người nhận chưa đọc")]
        NotRead,

        [Description("người nhận đã đọc")]
        Pending,

        [Description("người nhận đã chấp nhận lời mời kết bạn nhưng người gửi chưa biết")]
        Accepted,
    }
}