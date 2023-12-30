namespace BKConnectBE.Common.Enumeration
{
    public enum WebSocketDataType
    {
        IsMessage,
        IsNotification,
        IsOnline,
        IsOffline,
        IsChangedRoomInfo,
        IsVideoCall,
        IsCreateGroupRoom,
        IsError,
        IsConnectSignal,
        IsReloadChats,
    }
}