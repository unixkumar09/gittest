using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }
        [Required]
        public string DeviceNumber { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
