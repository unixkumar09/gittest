using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Vans_SRMS_API.ViewModels;
using Microsoft.Extensions.Logging;

namespace Vans_SRMS_API.Websockets
{
    public abstract class WebSocketHandler
    {
        private readonly ILogger<WebSocketHandler> _logger;
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager, ILogger<WebSocketHandler> logger)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
            _logger = logger;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);
            return;
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket)).ConfigureAwait(false);
        }

        public async Task SendMessageAsync(WebSocket socket, WebSocketMessage message)
        {
            if (socket == null)
                return;

            if (socket.State != WebSocketState.Open)
                return;

            var serializedMessage = JsonConvert.SerializeObject(message, _jsonSerializerSettings);
            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(serializedMessage),
                                                                  offset: 0,
                                                                  count: serializedMessage.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        public async Task SendMessageAsync(string socketId, WebSocketMessage message)
        {
            await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), message).ConfigureAwait(false);
        }

        public async Task SendMessageToAllAsync(WebSocketMessage message)
        {
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                try
                {
                    if (pair.Value.State == WebSocketState.Open)
                        await SendMessageAsync(pair.Value, message).ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    //todo: stop swallowing exception
                    // throw;
                    EventId evId = new EventId(DateTime.Now.Millisecond, pair.Key);
                    _logger.LogError(evId, ex, ex.Message, ex.StackTrace);
                  //  _logger.LogError(evId, ex, ex.InnerException.Message, ex.StackTrace);
                }
            }
        }

        //public async Task InvokeClientMethodAsync(string socketId, string methodName, object[] arguments)
        //{
        //    var message = new WebSocketMessage(
        //        JsonConvert.SerializeObject(new InvocationDescriptor()
        //        {
        //            MethodName = methodName,
        //            Arguments = arguments
        //        }, _jsonSerializerSettings));

        //    await SendMessageAsync(socketId, message).ConfigureAwait(false);
        //}

        //public async Task InvokeClientMethodToAllAsync(string methodName, params object[] arguments)
        //{
        //    foreach (var pair in WebSocketConnectionManager.GetAll())
        //    {
        //        if (pair.Value.State == WebSocketState.Open)
        //            await InvokeClientMethodAsync(pair.Key, methodName, arguments).ConfigureAwait(false);
        //    }
        //}

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

    }
}
