using Microsoft.AspNetCore.Mvc;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.ViewModels;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using Vans_SRMS_API.Filters;
using Vans_SRMS_API.Database;

namespace Vans_SRMS_API.Controllers
{
    [Route("api/[controller]")]
    public class DeviceController : Controller
    {
        private readonly SRMS_DbContext _context;
        private readonly IDeviceRepository _deviceRepo;

        public DeviceController(SRMS_DbContext dbContext, IDeviceRepository deviceRepo)
        {
            _context = dbContext;
            _deviceRepo = deviceRepo;
        }

        /// <summary>
        /// DEVELOPMENT TESTING ROUTE
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <returns></returns>
        /// <remarks>This route is for testing.  Should not be called from the handhelds.
        /// Use this route to lookup information on a registered device number</remarks>
        [HttpGet("{deviceNumber}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DeviceViewModel))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult Get(string deviceNumber)
        {
            return _deviceRepo.Find(deviceNumber).respond();

        }

        /// <summary>
        /// Use this route to associate a new handheld with the server.
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <returns></returns>
        /// <remarks>The server will assign a unique color and Identifier Key for this device.  
        /// Use the returned Identifier Key in the Auth header.
        /// If called twice from the same handheld, it will not register a duplicate device. 
        /// It will return the existing color and key.</remarks>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DeviceViewModel))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult Post([FromBody]DeviceNumber deviceNumber)
        {
            return _deviceRepo.Create(deviceNumber.deviceNumber).respond();
        }

        /// <summary>
        /// Use this route to deactivate a device for the default store
        /// </summary>
        /// <param name="id">database DeviceId</param>
        /// <returns></returns>
        /// <remarks>Uses the database DeviceId instead of device number in case of duplicate device numbers.</remarks>
        [HttpDelete("{id}")]
        [DeviceAuthorized]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult Delete(int id)
        {
            return _deviceRepo.Delete(id).respond();            
        }
    }
}
