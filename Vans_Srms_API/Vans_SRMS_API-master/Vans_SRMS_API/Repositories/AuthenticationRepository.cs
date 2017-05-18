using System;
using System.Linq;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.Database;
using Microsoft.EntityFrameworkCore;

namespace Vans_SRMS_API.Repositories
{
    public interface IAuthenticationRepository
    {
        int? DeviceKeyIsValid(string key);
    }

    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly SRMS_DbContext _context;

        public AuthenticationRepository(SRMS_DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Look up the store device by deviceKey, return the DeviceId from the Device table
        /// </summary>
        /// <param name="key"></param>
        /// <returns>DeviceId from Device table</returns>
        public int? DeviceKeyIsValid(string key)
        {
            //TODO: add caching option  
            Guid deviceKeyGuid;

            if (!Guid.TryParse(key, out deviceKeyGuid))
                return null;

            StoreDevice storeDevice = _context.StoreDevices
                .Include(sd=>sd.Device)
                .FirstOrDefault(sd => sd.DeviceKey == deviceKeyGuid && sd.Active);

            if (storeDevice == null)
                return null;

            // todo: return storeDeviceId instead of deviceId
            return storeDevice.Device.DeviceId;
        }
    }
}
