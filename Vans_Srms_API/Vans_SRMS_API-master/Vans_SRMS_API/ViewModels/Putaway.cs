using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.ViewModels
{
    public class PutawayInput
    {
        [Required]
        public List<string> GTINs { get; set; }
        [Required]
        public string LocationBarcode { get; set; }

        public PutawayInput() { }
    }

    public class PutawayResponse
    {
    }
}
