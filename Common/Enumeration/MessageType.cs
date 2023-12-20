using System.ComponentModel;

namespace BKConnectBE.Common.Enumeration
{
    public enum MessageType
    {
        [Description("văn bản")]
        Text,

        [Description("hình ảnh")]
        Image,

        [Description("tin nhắn thoại")]
        Audio,

        [Description("tệp đính kèm")]
        File,

        [Description("hệ thống")]
        System,
        
        [Description("bắt đầu cuộc gọi")]
        IsStartCall
    }
}