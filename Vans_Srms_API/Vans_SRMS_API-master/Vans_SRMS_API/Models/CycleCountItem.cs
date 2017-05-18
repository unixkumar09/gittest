using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class CycleCountItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CycleCountItemId { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public long CycleCountId { get; set; }
        public CycleCount CycleCount { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //public int Quantity { get; set; }
    }
}
