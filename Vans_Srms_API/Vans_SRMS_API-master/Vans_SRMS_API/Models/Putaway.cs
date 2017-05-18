using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class Putaway
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PutawayId { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }

        public Putaway(int productId, int locationId, int deviceId, DateTime dt)
        {
            Timestamp = dt;
            ProductId = productId;
            LocationId = locationId;
            DeviceId = deviceId;
        }
    }
}
