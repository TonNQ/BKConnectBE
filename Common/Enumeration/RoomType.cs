using System.ComponentModel;

namespace BKConnectBE.Common.Enumeration
{
    public enum RoomType
    {
        [Description("Phòng chat 2 người")]
        PrivateRoom,

        [Description("Phòng chat nhiều người")]
        PublicRoom,

        [Description("Lớp học")]
        ClassRoom,
    }
}