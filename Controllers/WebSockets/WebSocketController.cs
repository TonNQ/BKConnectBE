using BKConnect.Service.Jwt;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;
using BKConnectBE.Service.Users;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace BKConnect.Controllers;

[ApiController]
[Route("websocket")]
public class WebSocketController : ControllerBase
{
    private static List<WebSocketConnection> _websocketList;

    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public static List<WebSocketConnection> websocketList
    {
        get
        {
            if (_websocketList == null)
            {
                _websocketList = new List<WebSocketConnection>();
            }
            return _websocketList;
        }
    }

    public WebSocketController(IUserService userService, IJwtService jwtService)
    {
        _websocketList = websocketList;
        _userService = userService;
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
                _websocketList.Add(webSocketConnection);

                var websocketData = new WebsocketData
                {
                    UserId = tokenInfo["UserId"],
                    DataType = WebSocketDataType.IsOnline.ToString()
                };

                await SendStatusMessage(websocketData);

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
            var receivedObject = JsonSerializer.Deserialize<WebsocketData>(receivedData);

            buffer = new byte[1024 * 4];
            receiveResult = await cnn.WebSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await CloseConnection(cnn);
    }

    private async Task CloseConnection(WebSocketConnection cnn)
    {
        var websocketData = new WebsocketData
        {
            UserId = cnn.UserId,
            DataType = WebSocketDataType.IsOffline.ToString()
        };

        await _userService.UpdateLastOnlineAsync(cnn.UserId);
        await SendStatusMessage(websocketData);

        await cnn.WebSocket.CloseAsync(
            WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
    }

    private async Task SendStatusMessage(WebsocketData websocketData)
    {
        var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(websocketData));

        var tasks = new List<Task>();

        foreach (WebSocketConnection webSocket in _websocketList)
        {
            tasks.Add(webSocket.WebSocket.SendAsync(
                new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None));
        }
        await Task.WhenAll(tasks);
    }
}
