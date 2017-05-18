using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Barcode { get; set; }
        public string Description { get; set; }
        [Required]
        public bool FlaggedForCycleCount { get; set; }
        [Required]
        public bool FlaggedForAudit { get; set; }

        [Required]
        public int StoreId { get; set; }
        public Store Store { get; set; }

    }
}
