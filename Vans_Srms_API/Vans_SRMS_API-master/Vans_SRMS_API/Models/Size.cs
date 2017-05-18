using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class Size
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SizeId { get; set; }
        [Required]
        public float USSize { get; set; }
        public float WomenSize { get; set; }
        [Required]
        public bool Kids { get; set; }

        public float UKSize { get; set; }
        public float EURSize { get; set; }
        public float MEXSize { get; set; }
    }
}
