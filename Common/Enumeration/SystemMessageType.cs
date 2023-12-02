using System.ComponentModel;

namespace BKConnectBE.Common.Enumeration
{
    public enum SystemMessageType
    {
        [Description("thêm vào nhóm")]
        IsInRoom,

        [Description("xoá khỏi nhóm")]
        IsOutRoom,

        [Description("tạo nhóm")]
        IsCreateGroupRoom,
    }
}