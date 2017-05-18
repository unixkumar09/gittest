using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class ProductLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductLocationId { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime LastUpdatedAt { get; set; }
        [Required]
        [ForeignKey("LastUpdatedByDevice")]
        public int LastUpdatedBy { get; set; }
        public Device LastUpdatedByDevice { get; set; }
    }
}
