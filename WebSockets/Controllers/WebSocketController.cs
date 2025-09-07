using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace WebSockets.Controllers
{
    public class WebSocketController : Controller
    {
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
        private static async Task Echo(WebSocket webSocket)
        {
            //var buffer = new byte[1024 * 4];
            //var receiveResult = await webSocket.ReceiveAsync(
            //    new ArraySegment<byte>(buffer), CancellationToken.None);

            while (true)
            {
                var message = "Hello World";
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
