using System;

namespace Vans_SRMS_API.ViewModels
{
    public class DeviceNumber
    {
        public string deviceNumber { get; set; }
    }

    public class DeviceViewModel
    {
        public int DeviceId { get; set; }
        public string DeviceNumber { get; set; }
        public string DeviceColor { get; set; }
        public Guid DeviceKey { get; set; }
        public bool Active { get; set; }

        public DeviceViewModel(int id, string num, string color, Guid key, bool active)
        {
            DeviceId = id;
            DeviceNumber = num;
            DeviceColor = color;
            DeviceKey = key;
            Active = active;
        }
    }
}
