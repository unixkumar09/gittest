using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vans_SRMS_API.Models;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.ViewModels
{
    /// <summary>
    /// Use either the VN Number field, or the Style and Color fields
    /// </summary>
    public class ShoeDetailInput : IValidatableObject
    {
        public string VNNumber { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(VNNumber) && String.IsNullOrEmpty(Style) && String.IsNullOrEmpty(Color))
                yield return new ValidationResult("You must supply either a VNNumber or a combination of Style and Color");
        }
    }

    public class ShoeRequestInput
    {
        [Required]
        public string VNNumber { get; set; }
        [Required]
        public float Size { get; set; }
        [Required]
        public ShoeType Type { get; set; }
    }

    public class ShoeRequestResponse
    {
        public long RequestId { get; set; }
        public string VNNumber { get; set; }
        public float Size { get; set; }
        public ShoeType Type { get; set; }
        public int InStockQuantity { get; set; }

        public ShoeRequestResponse(Request request)
        {
            RequestId = request.RequestId;
            VNNumber = request.VNNumber;
            Size = request.Size;
            Type = request.Type;
            InStockQuantity = request.InStockQuantity;
        }
    }

    public class ShoeDetailResponse
    {
        public string Style { get; set; }
        public string Color { get; set; }

        public ShoeBasicViewModel Mens { get; set; }
        public ShoeBasicViewModel Womens { get; set; }
        public ShoeBasicViewModel Youth { get; set; }

        public ShoeDetailResponse(string style, string color)
        {
            Style = style;
            Color = color;
            Mens = new ShoeBasicViewModel();
            Womens = new ShoeBasicViewModel();
            Youth = new ShoeBasicViewModel();
        }
    }

    public class ShoeBasicViewModel
    {
        public string VNNumber { get; set; }
        public List<SizeResponse> Sizes { get; set; }

        public ShoeBasicViewModel()
        {
            Sizes = new List<SizeResponse>();
        }
        public void AddShoe(float size, int qty)
        {
            Sizes.Add(new SizeResponse() { Size = size, Quantity = qty });
        }
    }

    public class SizeResponse
    {
        public float Size { get; set; }
        public int Quantity { get; set; }
    }

    public class ShoeViewModel
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string VendorStyle { get; set; }
        public string SKU { get; set; }
        public ShoeType Type { get; set; }
        public decimal Retail { get; set; }
        public DateTime LastUpdate { get; set; }

        public string ItemNumber { get; set; }
        public string GTIN { get; set; }
        public float Size { get; set; }

        public ShoeViewModel(Product product)
        {
            ProductId = product.ProductId;
            Description = product.Description;
            VendorStyle = product.VendorStyle;
            SKU = product.SKU;
            Style = product.Style;
            Color = product.Color;
            Type = product.Type;
            Retail = product.Retail;
            LastUpdate = product.LastUpdate;
            ItemNumber = product.ItemNumber;
            GTIN = unpadGTIN(product.GTIN);
            Size = product.Size;
        }
    }

    public class StyleColorOptions
    {
        public List<StyleOption> Styles { get; set; }

        public StyleColorOptions()
        {
            Styles = new List<StyleOption>();
        }
    }

    public class StyleOption
    {
        public string Style { get; set; }
        public List<string> Colors { get; set; }

        public StyleOption()
        {
            Colors = new List<string>();
        }
    }

}
