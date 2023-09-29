using System.ComponentModel;

namespace BKConnectBE.Common.Enumeration
{
    public enum MessageType
    {
        [Description("Văn bản")]
        Text,

        [Description("Hình ảnh")]
        Image,

        [Description("Âm thanh")]
        Audio,

        [Description("Tệp")]
        File
    }
}