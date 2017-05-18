using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.Filters;
using Vans_SRMS_API.ViewModels;
using Vans_SRMS_API.Utils;

namespace Vans_SRMS_API.Controllers
{
    [Route("api/[controller]")]
    [DeviceAuthorized]
    [SwaggerResponse(401, Type = typeof(string), Description = "Device key authorization failed")]
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepo;

        public LocationController(ILocationRepository locationRepo)
        {
            _locationRepo = locationRepo;
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        /// <remarks>Grab any random locations from the database for testing purposes.</remarks>
        [HttpGet("Random")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<Location>), Description = "Locations")]
        public IActionResult Random(int skip = 0, int take = 10)
        {
            return _locationRepo.GetSome(skip, take).respond();
        }

        /// <summary>
        /// Use this route to overwrite existing inventory records for a specific location
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>Pass a single Location barcode, any number of Product GTIN barcodes, and a timestamp for each Product scan.  
        /// We'll record a timestamp for each Product in order to track the time it takes to scan each Product.</remarks>
        [HttpPost("CycleCount")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string), Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string), Description = "Invalid Location or Product")]
        public IActionResult CycleCount([FromBody]CycleCountInput input)
        {
            return _locationRepo.CycleCount(input, Util.getDeviceId(HttpContext)).respond();
        }

        /// <summary>
        /// Use this route to perform an audit on a Location.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>Current inventory location will be compared to the system of record
        /// and a report will be generated with overages and shortages.</remarks>
        [HttpPost("Audit")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type =typeof(string), Description = "Audit successful")]
        public IActionResult Audit([FromBody] CycleCountInput input)
        {
            return _locationRepo.Audit(input, Util.getDeviceId(HttpContext)).respond();
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <param name="locationBarcode"></param>
        /// <returns></returns>
        /// <remarks>Used to get details on the products inside of a location</remarks>
        [HttpGet("Details")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LocationViewModel), Description = "Location Details")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string), Description = "Invalid Location or Product")]
        public IActionResult Details(string locationBarcode)
        {
            return _locationRepo.Details(locationBarcode).respond();
        }

        [HttpGet("BackupReport")]
        public IActionResult BackupReport()
        {
            return _locationRepo.GenerateBackupFile().respond();
        }
    }
}
