using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class StoreDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreDeviceId { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public Guid DeviceKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdate { get; set; }

        [Required]
        public int StoreId { get; set; }
        public Store Store { get; set; }
        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        [Required]
        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}
