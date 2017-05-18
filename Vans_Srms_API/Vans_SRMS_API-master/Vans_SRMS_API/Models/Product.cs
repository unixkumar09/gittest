using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vans_SRMS_API.ViewModels;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        [Required]
        public string VendorStyle { get; set; }
        [Required]
        public string GTIN { get; set; }
        public string SKU { get; set; }

        [Required]
        public string Style { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public float Size { get; set; }
        [Required]
        public float ConvertedSize { get; set; }
        // [Required]
        public ShoeType Type { get; set; }
        public decimal Retail { get; set; }

        // used for quicker searching when retrieving all Style/Color combinations
        public bool InStock { get; set; }

        public DateTime LastUpdate { get; set; }

        public ProductViewModel toViewModel()
        {
            return new ProductViewModel()
            {
                ProductId = ProductId,
                ItemNumber = ItemNumber,
                Description = Description,
                VendorStyle = VendorStyle,
                GTIN = GTIN,
                SKU = SKU,
                Style = Style,
                Color = Color,
                Size = Size,
                Type = Type,
                Retail = Retail,
                InStock = InStock,
                LastUpdate = LastUpdate
            };
        }

        public bool IsEqualTo(Product p2)
        {
            return (
                ItemNumber == p2.ItemNumber &&
                Description == p2.Description &&
                VendorStyle == p2.VendorStyle &&
                SKU == p2.SKU &&
                Style == p2.Style &&
                Color == p2.Color &&
                Type == p2.Type &&
                ConvertedSize == p2.ConvertedSize &&
                Retail == p2.Retail);
        }
    }
}
