using System.ComponentModel.DataAnnotations;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.ViewModels
{
    public class PickInput
    {
        [Required]
        public string GTIN { get; set; }
        [Required]
        public string LocationBarcode { get; set; }
        public long OrderId { get; set; }
    }
}
