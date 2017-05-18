using Microsoft.AspNetCore.Mvc;
using Vans_SRMS_API.Filters;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.ViewModels;
using Vans_SRMS_API.Utils;
using Vans_SRMS_API.Database;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using static Vans_SRMS_API.Utils.Util;
using Vans_SRMS_API.Models;

namespace Vans_SRMS_API.Controllers
{
    [Route("api/[controller]")]
    [DeviceAuthorized]
    public class ShoeController : Controller
    {
        private readonly SRMS_DbContext _context;
        private readonly IShoeRepository _shoeRepo;

        public ShoeController(SRMS_DbContext dbContext, IShoeRepository shoeRepo)
        {
            _context = dbContext;
            _shoeRepo = shoeRepo;
        }

        /// <summary>
        /// Use this route to look up details of a particular shoe, including available sizes with in-stock quantities
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("Detail")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ShoeDetailResponse), Description = "Includes details about the shoe, including all sizes and in-stock quantities for all sizes")]
        public IActionResult Detail(ShoeDetailInput input)
        {
            ShoeDetailResponse response = _shoeRepo.Detail(input, Util.getDeviceId(HttpContext));

            return Ok(response);
        }

        /// <summary>
        /// Use this route when a customer requests a shoe in a particular size.
        /// This will record the request for reporting purposes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>This would normally look up in-stock quantities for the shoe, but we are returning that data in the /api/Shoe/Detail route.  This route is still needed to track requests however.</remarks>
        [HttpPost("Request")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ShoeRequestResponse), Description = "Record the RequestId from this response and include it when placing an order")]
        public IActionResult Request(ShoeRequestInput input)
        {
            return _shoeRepo.Request(input, Util.getDeviceId(HttpContext)).respond();
        }

        /// <summary>
        /// Use this route to print labels for display models. 
        /// Look up item information by GTIN number.
        /// </summary>
        /// <param name="GTIN"></param>
        /// <returns>ShoeViewModel</returns>
        [HttpGet("GTIN")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ShoeViewModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult GTIN(string GTIN)
        {
            ShoeViewModel shoe = _shoeRepo.GetInfoByGTIN(padGTIN(GTIN));

            if (shoe == null)
                return NotFound(string.Format("GTIN: {0} not found.", GTIN));

            return Ok(shoe);
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <param name="VN"></param>
        /// <returns></returns>
        /// <remarks>Same process as GTIN lookup, but searches by VN Number intead.</remarks>
        [HttpGet("VN")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ShoeViewModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult VN(string VN)
        {
            ShoeViewModel shoe = _shoeRepo.GetInfoByVN(VN);

            if (shoe == null)
                return NotFound(string.Format("VN Number: {0} not found.", VN));

            return Ok(shoe);

        }

        /// <summary>
        /// Use this route to fill the dropdown lists for Style and Color
        /// </summary>
        /// <returns></returns>
        [HttpGet("Options")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(StyleColorOptions))]
        public IActionResult Options()
        {
            return Ok(_shoeRepo.GetStyleColorOptions());
        }
    }
}
