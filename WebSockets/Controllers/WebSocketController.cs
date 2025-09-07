using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using WebSockets.Models;

namespace WebSockets.Controllers
{
    public class WebSocketController : Controller
    {
        private readonly IApiClient _client;

        public WebSocketController(IApiClient client)
        {
            _client = client;
        }

        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        private async Task Echo(WebSocket webSocket)
        {
            //var buffer = new byte[1024 * 4];
            //var receiveResult = await webSocket.ReceiveAsync(
            //    new ArraySegment<byte>(buffer), CancellationToken.None);

            while (true)
            {
                string message = await _client.GetSessions();

                var bytes = Encoding.UTF8.GetBytes(message);

                await webSocket.SendAsync(
                    new ArraySegment<byte>(bytes, 0, bytes.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);

                if(webSocket.State == WebSocketState.Closed || webSocket.State == WebSocketState.Aborted)
                {
                    break;
                }

                await Task.Delay(10000);

                //receiveResult = await webSocket.ReceiveAsync(
                //    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                WebSocketCloseStatus.Empty,
                string.Empty,
                CancellationToken.None);
        }
    }
}
