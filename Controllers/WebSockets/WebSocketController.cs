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
                    UserId = tokenInfo["UserId"],
                    WebSocket = webSocket
                };
                _webSocketService.AddWebSocketConnection(webSocketConnection);

                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = tokenInfo["UserId"],
                    DataType = WebSocketDataType.IsOnline.ToString()
                };

                await _webSocketService.SendStatusMessage(receiveWebSocketData);

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
        var buffer = new byte[1024 * 4];
        var receiveResult = await cnn.WebSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!cnn.WebSocket.CloseStatus.HasValue)
        {
            var receivedData = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            var receivedObject = JsonSerializer.Deserialize<SendWebSocketData>(receivedData);

            if (receivedObject.DataType == "IsMessage")
            {
                await _webSocketService.SendTextMessage(receivedObject, cnn.UserId);
            }
            else if (receivedObject.DataType == "IsFriendRequest")
            {
                await _webSocketService.SendFriendRequest(receivedObject, cnn.UserId);
            }

            buffer = new byte[1024 * 4];
            receiveResult = await cnn.WebSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await _webSocketService.CloseConnection(cnn);
    }
}
