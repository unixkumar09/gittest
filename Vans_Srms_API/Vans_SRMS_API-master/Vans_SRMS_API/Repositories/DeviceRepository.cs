using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.ViewModels;
using Vans_SRMS_API.Database;
using System.Net;

namespace Vans_SRMS_API.Repositories
{
    public interface IDeviceRepository
    {
        RepoResponse<DeviceViewModel> Find(string deviceNumber);
        RepoResponse<DeviceViewModel> Create(string deviceNumber);
        RepoResponse<string> Delete(int id);
    }
    public class DeviceRepository : IDeviceRepository
    {
        private readonly SRMS_DbContext _context;

        public DeviceRepository(SRMS_DbContext context)
        {
            _context = context;
        }
        
        public RepoResponse<DeviceViewModel> Find(string deviceNumber)
        {
            if (deviceNumber == null)
                return new RepoResponse<DeviceViewModel>(HttpStatusCode.BadRequest, "Device number cannot be null");

            StoreDevice storeDevice = _context.StoreDevices
            .Include(sd => sd.Device)
            .Include(sd => sd.Color)
            .FirstOrDefault(sd => sd.Device.DeviceNumber == deviceNumber &&
            sd.Active);

            if (storeDevice == null)
                return new RepoResponse<DeviceViewModel>(HttpStatusCode.BadRequest, "Device not found");

            DeviceViewModel response = new DeviceViewModel(storeDevice.Device.DeviceId, storeDevice.Device.DeviceNumber, storeDevice.Color.Hex, storeDevice.DeviceKey, storeDevice.Active);
            return new RepoResponse<DeviceViewModel>(HttpStatusCode.OK, response);
           
        }
        
        public RepoResponse<DeviceViewModel> Create(string deviceNumber)
        {
            if (deviceNumber == null)
                return new RepoResponse<DeviceViewModel>(HttpStatusCode.BadRequest, "Device number cannot be null");

            // look for existing object
            StoreDevice storeDevice = _context.StoreDevices
                .Include(sd => sd.Device)
                .Include(sd => sd.Color)
                .FirstOrDefault(sd => sd.Device.DeviceNumber == deviceNumber && sd.Active);

            // create new device object
            if (storeDevice == null)
            {
                try
                {
                    storeDevice = createNewDevice(deviceNumber);
                }
                catch (Exception ex)
                {
                    return new RepoResponse<DeviceViewModel>(HttpStatusCode.BadRequest, ex.Message);
                }
            }

            // return a viewmodel for the front end
            DeviceViewModel returnObject = new DeviceViewModel(storeDevice.Device.DeviceId, storeDevice.Device.DeviceNumber, storeDevice.Color.Hex, storeDevice.DeviceKey, storeDevice.Active);

            return new RepoResponse<DeviceViewModel>(HttpStatusCode.OK, returnObject);
        }

        private StoreDevice createNewDevice(string deviceNumber)
        {
            if (deviceNumber == null)
                throw new Exception("Device number cannot be null.  Make sure it was sent with quotation marks around it.");

            // find store ID
            Store defaultStore = _context.Stores.First(s => s.IsDefault);
            if (defaultStore == null)
                throw new Exception("Default store not found.  You must set a default store first.");


            // create new device
            Device device = new Device { DeviceNumber = deviceNumber };
            _context.Devices.Add(device);
            int result = _context.SaveChanges();
            if (result == 0)
                throw new Exception("Device could not be created");
            _context.Entry(device).Reload();


            // pick an unused color for the new device
            // if all colors are used, grab a random color
            IEnumerable<int> usedColors = _context.StoreDevices.Where(sd => sd.Active).Select(c => c.ColorId).Distinct();
            Color color = _context.Colors.Where(c => !usedColors.Contains(c.ColorId)).FirstOrDefault();
            if (color == null)
                color = getRandomColor();


            // assign the new device to the default store
            DateTime now = DateTime.Now;
            StoreDevice storeDevice = new StoreDevice
            {
                Active = true,
                DeviceKey = new Guid(),
                CreatedAt = now,
                LastUpdate = now,
                StoreId = defaultStore.StoreId,
                DeviceId = device.DeviceId,
                ColorId = color.ColorId
            };
           
            _context.StoreDevices.Add(storeDevice);
            result = _context.SaveChanges();
            if (result == 0)
                throw new Exception("Device could not be assigned to default store");
            _context.Entry(storeDevice).Reload();

            return storeDevice;

            Color getRandomColor()
            {
                Random r = new Random();
                int skip = r.Next(1, _context.Colors.Count());
                return _context.Colors.Skip(skip).Take(1).First();
            }

        }
        
        public RepoResponse<string> Delete(int id)
        {
            StoreDevice storeDevice = _context.StoreDevices
                .Include(sd => sd.Device)
                .FirstOrDefault(sd => sd.Device.DeviceId == id && sd.Active);

            if (storeDevice == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest, "Device not found");

            storeDevice.Active = false;
            _context.SaveChanges();

            return new RepoResponse<string>(HttpStatusCode.OK, "Device deleted");
        }
    }
}
