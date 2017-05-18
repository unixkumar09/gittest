using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.ViewModels;

namespace Vans_SRMS_API.Websockets
{
    public class OrderMessageHandler : WebSocketHandler
    {
        IOrderRepository _orderRepo;
        private readonly ILogger<OrderMessageHandler> _logger;
        public OrderMessageHandler(WebSocketConnectionManager webSocketConnectionManager, IOrderRepository orderRepo, ILogger<OrderMessageHandler> logger)
            : base(webSocketConnectionManager, logger)
        {
            _orderRepo = orderRepo;
            _logger = logger;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            string socketId = WebSocketConnectionManager.GetId(socket);
            if (string.IsNullOrEmpty(socketId))
                return;

            await sendUpdatesToSocket(socketId);
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            WebSocketMessage request;
            string requestData;

            var socketId = WebSocketConnectionManager.GetId(socket);
            if (string.IsNullOrEmpty(socketId))
                return;

            await sendUpdatesToSocket(socketId);
        }

        public async Task sendUpdatesToSocket(string socketId)
        {
            try
            {
                //var orders = _orderRepo.GetAll();
                //WebSocketMessage response = new WebSocketMessage(orders);
                RepoResponse<List<OrderResponse>> orders = _orderRepo.GetAll();
                var message = new WebSocketMessage(orders.responseObject);

                await SendMessageAsync(socketId, message);
            }
            catch (Exception ex)
            {
                //todo: stop swallowing exception
                // throw;

                EventId evId = new EventId(DateTime.Now.Millisecond, socketId);
                _logger.LogError(evId, ex, ex.Message, ex.StackTrace);
             //   _logger.LogError(evId, ex, ex.InnerException.Message, ex.StackTrace);
            }
        }

        public async Task broadcastUpdates()
        {
            try
            {
                RepoResponse<List<OrderResponse>> orders = _orderRepo.GetAll();
                var message = new WebSocketMessage(orders.responseObject);

                await SendMessageToAllAsync(message);
            }
            catch (System.Exception ex)
            {
                //todo: stop swallowing exceptions
                // this is to keep the server from shutting down while we work on websockets
                EventId evId = new EventId(DateTime.Now.Millisecond, "broadcast");
                _logger.LogError(evId, ex, ex.Message, ex.StackTrace);
             //   _logger.LogError(evId, ex, ex.InnerException.Message, ex.StackTrace);
            }
        }
    }
}
