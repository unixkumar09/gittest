using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Vans_SRMS_API.Websockets
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler { get; set; }
        private ILogger<WebSocketManagerMiddleware> _logger;

        public WebSocketManagerMiddleware(RequestDelegate next,
                                          WebSocketHandler webSocketHandler,
                                          ILogger<WebSocketManagerMiddleware> logger)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                //await _next.Invoke(context);
                return;
            }

            using (var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false))
            {
                //var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
                await _webSocketHandler.OnConnected(socket).ConfigureAwait(false);

                await Receive(socket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        await _webSocketHandler.ReceiveAsync(socket, result, buffer).ConfigureAwait(false);
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocketHandler.OnDisconnected(socket).ConfigureAwait(false);
                        return;
                    }
                });

                //TODO - investigate the Kestrel exception thrown when this is the last middleware
                // await _next.Invoke(context);
            }

        }
        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            try
            {

                var buffer = new byte[1024 * 4];

                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                           cancellationToken: CancellationToken.None);

                    handleMessage(result, buffer);
                }
            }
            catch (Exception ex)
            {
                EventId evId = new EventId(DateTime.Now.Millisecond, "WebSocketManagerMiddleware");
                _logger.LogError(evId, ex, ex.Message, ex.StackTrace);
               // _logger.LogError(evId, ex, ex.InnerException.Message, ex.StackTrace);
            }
        }
        //private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
        //{
        //    while (socket.State == WebSocketState.Open)
        //    {
        //        ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
        //        string serializedInvocationDescriptor = null;
        //        WebSocketReceiveResult result = null;
        //        using (var ms = new MemoryStream())
        //        {
        //            do
        //            {
        //                result = await socket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
        //                ms.Write(buffer.Array, buffer.Offset, result.Count);
        //            }
        //            while (!result.EndOfMessage);

        //            ms.Seek(0, SeekOrigin.Begin);

        //            using (var reader = new StreamReader(ms, Encoding.UTF8))
        //            {
        //                serializedInvocationDescriptor = await reader.ReadToEndAsync().ConfigureAwait(false);
        //            }
        //        }

        //        handleMessage(result, serializedInvocationDescriptor);
        //    }
        //}
    }
}
