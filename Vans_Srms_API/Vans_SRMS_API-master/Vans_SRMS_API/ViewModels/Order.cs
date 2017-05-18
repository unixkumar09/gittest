using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vans_SRMS_API.Models;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.ViewModels
{
    public class OrderCreateInput
    {
        [Required]
        public List<OrderInput> Orders { get; set; }
    }

    public class OrderInput
    {
        [Required]
        public string VNNumber { get; set; }
        [Required]
        public float Size { get; set; }
        [Required]
        [EnumDataType(typeof(ShoeType))]
        public ShoeType Type { get; set; }
        [Required]
        public long RequestId { get; set; }
    }
    
    public class OrderResponse
    {
        private string _gtin;

        public long OrderId { get; set; }
        public string Hex { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string VNNumber { get; set; }
        public string GTIN
        {
            get { return unpadGTIN(_gtin); }
            set { _gtin = value; }
        }
        public float Size { get; set; }
        public List<LocationResponse> Locations { get; set; }

        public OrderResponse()
        {
            Locations = new List<LocationResponse>();
        }
    }
}
