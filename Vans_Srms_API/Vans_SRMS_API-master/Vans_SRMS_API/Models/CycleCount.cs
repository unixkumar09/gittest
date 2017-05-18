using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class CycleCount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CycleCountId { get; set; }
       
        //todo: remove StoreId since we have it in the Location field
        public int StoreId { get; set; }
        public Store Store { get; set; }
        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }

        List<CycleCountItem> CycleCountItems { get; set; }
    }
}
