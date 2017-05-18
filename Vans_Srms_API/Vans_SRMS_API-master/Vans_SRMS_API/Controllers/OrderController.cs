using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.Utils;
using Vans_SRMS_API.ViewModels;
using Vans_SRMS_API.Database;
using Vans_SRMS_API.Websockets;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Vans_SRMS_API.Filters;
using Microsoft.Extensions.Logging;

namespace Vans_SRMS_API.Controllers
{
    [Route("api/[controller]")]
    [DeviceAuthorized]
    [SwaggerResponse(401, Type = typeof(string), Description = "Device key authorization failed")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepo;
        private readonly OrderMessageHandler _wsHandler;

        public OrderController(ILogger<OrderController> logger, IOrderRepository orderRepo, OrderMessageHandler wsHandler)
        {
            _logger = logger;
            _orderRepo = orderRepo;
            _wsHandler = wsHandler;
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        [SwaggerResponse((int)HttpStatusCode.OK,
            Type = typeof(List<OrderResponse>),
            Description = "Get all orders for the default store")]
        public IActionResult Get(bool active = true)
        {
            return _orderRepo.GetAll(active).respond();
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK,
            Type = typeof(OrderResponse),
            Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest,
            Type = typeof(string),
            Description = "Order ID could not be found")]
        public IActionResult Get(int id)
        {
            return _orderRepo.GetOne(id).respond();
        }

        /// <summary>
        /// Use this route to place an order for a shoe to be picked from the stockroom
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK,
            Type = typeof(string),
            Description = "Returns an empty response, but initiates a websocket broadcast to all connected devices with the current active order list")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest,
            Type = typeof(string),
            Description = "Order could not be created.  Product is either invalid or out of stock.")]
        public async Task<IActionResult> Post([FromBody]OrderCreateInput input)
        {
            RepoResponse<string> response = await _orderRepo.Create(input, Util.getDeviceId(HttpContext));

            if (response.isSuccess)
                await _wsHandler.broadcastUpdates();

            return response.respond();
        }

        /// <summary>
        /// Use this route to cancel an open order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK,
            Type = typeof(string),
            Description = "Order Deleted")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest,
            Type = typeof(string),
            Description = "Order ID could not be found")]
        public async Task<IActionResult> Delete(int id)
        {
            RepoResponse<string> response = await _orderRepo.Delete(id, Util.getDeviceId(HttpContext));

            if (response.isSuccess)
                await _wsHandler.broadcastUpdates();

            return Ok();
        }

    }
}
