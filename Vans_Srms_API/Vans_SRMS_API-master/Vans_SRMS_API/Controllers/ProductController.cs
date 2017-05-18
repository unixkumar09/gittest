using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.Models;
using static Vans_SRMS_API.Utils.Util;
using Vans_SRMS_API.ViewModels;
using Vans_SRMS_API.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Vans_SRMS_API.Websockets;
using Microsoft.Extensions.Logging;

namespace Vans_SRMS_API.Controllers
{
    [Route("api/[controller]")]
    [SwaggerResponse(401, Type = typeof(string), Description = "Device key authorization failed")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly OrderMessageHandler _wsHandler;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepo, OrderMessageHandler wsHandler, ILogger<ProductController> logger)
        {
            _productRepo = productRepo;
            _wsHandler = wsHandler;
            _logger = logger;
        }

        /// <summary>
        /// Use this route to perform the bulk putaway process
        /// </summary>
        /// <param name="putaway"></param>
        /// <returns></returns>
        [HttpPost("Putaway")]
        [DeviceAuthorized]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string), Description = "Success")]
        public IActionResult Putaway([FromBody]PutawayInput putaway)
        {
            putaway.GTINs = padGTINs(putaway.GTINs);
            return _productRepo.Putaway(putaway, getDeviceId(HttpContext)).respond();
        }

        /// <summary>
        /// Use this route for both Open Picks and Order Fill Picks
        /// </summary>
        /// <param name="pick"></param>
        /// <returns></returns>
        [HttpPost("Pick")]
        [DeviceAuthorized]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string), Description = "Success")]
        public IActionResult Pick([FromBody]PickInput pick)
        {
            pick.GTIN = padGTIN(pick.GTIN);
            RepoResponse<string> response = _productRepo.Pick(pick, getDeviceId(HttpContext));

            if (response.notes == response._BROADCAST_ORDER_UPDATES)
                _wsHandler.broadcastUpdates();

            return response.respond();
        }

        /// <summary>
        /// Used for importing the Item Master List
        /// </summary>
        /// <returns></returns>
        /// <remarks>Will kick off an async process that will continue running after the request has timed out</remarks>
        [HttpPut("Import")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ItemMasterImportLog), Description = "Success")]
        public async Task<IActionResult> Import()
        {
            RepoResponse<ItemMasterImportLog> response = await _productRepo.ImportAll();
            return response.respond();
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <returns></returns>
        [HttpGet("Count")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(int), Description = "Product Count")]
        public IActionResult Count()
        {
            return _productRepo.Count().respond();
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        /// <remarks>Grab any random products from the database for testing purposes.</remarks>
        [HttpGet("Random")]
        [DeviceAuthorized]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<ProductViewModel>), Description = "Products")]
        public IActionResult Random(int skip = 0, int take = 10)
        {
            return _productRepo.GetSome(skip, take).respond();
        }
    }
}
