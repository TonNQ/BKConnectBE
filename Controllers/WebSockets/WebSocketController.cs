using BKConnect.BKConnectBE.Common;
using BKConnect.Service.Jwt;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;
using BKConnectBE.Service.WebSocket;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BKConnect.Controllers;

[ApiController]
[Route("websocket")]
public class WebSocketController : ControllerBase
{
    private readonly IWebSocketService _webSocketService;
    private readonly IJwtService _jwtService;

    public WebSocketController(IWebSocketService webSocketService, IJwtService jwtService)
    {
        _webSocketService = webSocketService;
        _jwtService = jwtService;
    }

    [HttpGet("ws")]
    public async Task Get(string accessToken)
    {
        try
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var tokenInfo = _jwtService.DecodeToken(accessToken);
                var webSocketConnection = new WebSocketConnection
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = tokenInfo["UserId"],
                    WebSocket = webSocket
                };

                if (_webSocketService.AddWebSocketConnection(webSocketConnection))
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = tokenInfo["UserId"],
                        DataType = WebSocketDataType.IsOnline.ToString()
                    };

                    await _webSocketService.SendStatusMessage(receiveWebSocketData, webSocketConnection.Id);
                }

                await Echo(webSocketConnection);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        catch (Exception) { }
    }

    private async Task Echo(WebSocketConnection cnn)
    {
        try
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await cnn.WebSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!cnn.WebSocket.CloseStatus.HasValue)
            {
                var receivedData = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                var receivedObject = JsonSerializer.Deserialize<SendWebSocketData>(receivedData);

                if (receivedObject.DataType == WebSocketDataType.IsMessage.ToString())
                {
                    await _webSocketService.SendMessage(receivedObject, cnn);
                }
                else if (receivedObject.DataType == WebSocketDataType.IsNotification.ToString())
                {
                    await _webSocketService.SendNotification(receivedObject, cnn);
                }
                else if (receivedObject.DataType == WebSocketDataType.IsVideoCall.ToString())
                {
                    await _webSocketService.CallVideo(receivedObject, cnn);
                }
                else if (receivedObject.DataType == WebSocketDataType.IsConnectSignal.ToString())
                {
                    await _webSocketService.SendSignalForVideoCall(receivedObject, cnn);
                }
                else
                {
                    await _webSocketService.SendErrorNotification(cnn, MsgNo.ERROR_WEBSOCKET_DATA);
                }

                buffer = new byte[1024 * 4];
                receiveResult = await cnn.WebSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await _webSocketService.CloseConnection(cnn);
        }
        catch (Exception)
        {
            await _webSocketService.CloseConnection(cnn);
        }
    }
}
