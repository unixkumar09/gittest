using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }
        [Required]
        public string VNNumber { get; set; }
        [Required]
        public float Size { get; set; }
        [Required]
        public float ConvertedSize { get; set; }
        [Required]
        [EnumDataType(typeof(ShoeType))]
        public ShoeType Type { get; set; }
        [Required]
        public DateTime LastUpdatedAt { get; set; }
        [Required]
        [ForeignKey("LastUpdatedByDevice")]
        public int LastUpdatedBy { get; set; }
        public Device LastUpdatedByDevice { get; set; }
        [Required]
        public DateTime OrderedAt { get; set; }
        [Required]
        [ForeignKey("OrderedByDevice")]
        public int OrderedBy { get; set; }
        public Device OrderedByDevice { get; set; }
        [ForeignKey("Request")]
        public long? RequestId { get; set; }
        public Request Request { get; set; }

        public int SuggestedLocationId { get; set; }
        [Required]
        public int StoreId { get; set; }
        public Store Store { get; set; }

        [Required]
        public bool Active { get; set; }
        public bool Picked { get; set; }
        public DateTime? PickedAt { get; set; }
        [ForeignKey("PickedByDevice")]
        public int? PickedBy { get; set; }
        public Device PickedByDevice { get; set; }
        [ForeignKey("PickedProduct")]
        public int? PickedProductId { get; set; }
        public Product PickedProduct { get; set; }
    }
}
