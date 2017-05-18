using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vans_SRMS_API.ViewModels;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RequestId { get; set; }
        [Required]
        public string VNNumber { get; set; }
        [Required]
        public float Size { get; set; }
        [Required]
        public ShoeType Type { get; set; }
        [Required]
        public DateTime RequestedAt { get; set; }
        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        [Required]
        public int StoreId { get; set; }
        public Store Store { get; set; }

        public int InStockQuantity { get; set; }
        
    }

}
