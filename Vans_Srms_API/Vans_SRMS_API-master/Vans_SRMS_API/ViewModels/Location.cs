using System.Collections.Generic;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.ViewModels
{
    public class LocationViewModel
    {
        public string LocationName { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public List<LocationProducts> Products { get; set; }

        public class LocationProducts
        {
            public int ProductId { get; set; }
            public string GTIN { get; set; }
            public string VNNumber { get; set; }
            public string Style { get; set; }
            public string Color { get; set; }
            public float Size { get; set; }
            public ShoeType Type { get; set; }
            public int Quantity { get; set; }

        }
    }
    public class LocationResponse
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
    }

    public class LocationBackupRecord
    {
        public string StoreNumber { get; set; }
        public string LocationBarcode { get; set; }
        public string GTIN { get; set; }
        public string VendorStyle { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public float Size { get; set; }
        public int Quantity { get; set; }
    }
}
