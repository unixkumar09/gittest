using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string VendorStyle { get; set; }
        public string GTIN { get; set; }
        public string SKU { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public float Size { get; set; }
        public ShoeType Type { get; set; }
        public decimal Retail { get; set; }
        public bool InStock { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
