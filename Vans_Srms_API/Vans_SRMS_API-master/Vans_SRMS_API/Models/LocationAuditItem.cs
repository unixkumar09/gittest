using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class LocationAuditItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LocationAuditItemId { get; set; }
        [Required]
        public long LocationAuditId { get; set; }
        public LocationAudit LocationAudit { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int ScannedQuantity { get; set; }
        [Required]
        public int OnRecordQuantity { get; set; }
    }
}
