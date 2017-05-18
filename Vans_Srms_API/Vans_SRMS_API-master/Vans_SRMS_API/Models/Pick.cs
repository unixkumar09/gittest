using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class Pick
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PickId { get; set; }
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

        public long? OrderId { get; set; }
        public Order Order { get; set; }
    }
}
