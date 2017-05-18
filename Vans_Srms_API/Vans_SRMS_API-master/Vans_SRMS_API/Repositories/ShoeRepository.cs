using System;
using System.Linq;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.ViewModels;
using static Vans_SRMS_API.Utils.Util;
using Vans_SRMS_API.Database;
using System.Net;
using System.Collections.Generic;

namespace Vans_SRMS_API.Repositories
{
    public interface IShoeRepository
    {
        ShoeDetailResponse Detail(ShoeDetailInput request, int deviceId);
        RepoResponse<ShoeRequestResponse> Request(ShoeRequestInput request, int deviceId);
        ShoeViewModel GetInfoByVN(string VNNumber);
        ShoeViewModel GetInfoByGTIN(string GTIN);
        StyleColorOptions GetStyleColorOptions();
    }
    public class ShoeRepository : IShoeRepository
    {
        private readonly SRMS_DbContext _context;

        public ShoeRepository(SRMS_DbContext context)
        {
            _context = context;
        }

        public ShoeDetailResponse Detail(ShoeDetailInput request, int deviceId)
        {
            return createShoeDetailResponseFromShoeDetailInput(request);
        }

        public RepoResponse<ShoeRequestResponse> Request(ShoeRequestInput request, int deviceId)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.VendorStyle == request.VNNumber
                && p.Size == request.Size
                && p.Type == request.Type);

            //if (product == null)
            //    return new RepoResponse<string>(HttpStatusCode.BadRequest,
            //        $"Could not find a shoe with VN#: {request.VNNumber} in size: {request.Size} for type: {request.Type}");

            int inStockQuantity = (product == null) ? 0 : _context.ProductLocations
                .Where(i => i.ProductId == product.ProductId)
                .Sum(i => i.Quantity);

            Store defaultStore = _context.Stores.FirstOrDefault(s => s.IsDefault);
            Request newRequest = new Request()
            {
                VNNumber = request.VNNumber,
                Size = request.Size,
                Type = request.Type,
                InStockQuantity = inStockQuantity,
                RequestedAt = DateTime.Now,
                StoreId = (defaultStore == null) ? -1 : defaultStore.StoreId,
                DeviceId = deviceId
            };
            _context.Requests.Add(newRequest);
            _context.SaveChanges();
            _context.Entry(newRequest).Reload();

            return new RepoResponse<ShoeRequestResponse>(HttpStatusCode.OK, new ShoeRequestResponse(newRequest));
        }

        public ShoeViewModel GetInfoByVN(string VNNumber)
        {
            if (string.IsNullOrEmpty(VNNumber))
                throw new Exception("Invalid VN Number");

            Product shoe = _context.Products.FirstOrDefault(p => p.VendorStyle == VNNumber);

            if (shoe == null)
                return null;

            return new ShoeViewModel(shoe);
        }

        public ShoeViewModel GetInfoByGTIN(string GTIN)
        {
            if (string.IsNullOrEmpty(GTIN))
                throw new Exception("Invalid GTIN number");

            Product shoe = _context.Products.FirstOrDefault(p => p.GTIN == GTIN);

            if (shoe == null)
                return null;

            return new ShoeViewModel(shoe);
        }

        public StyleColorOptions GetStyleColorOptions()
        {
            StyleColorOptions results = new StyleColorOptions();

            var allStyles = _context.Products
                .Where(p => p.InStock)
                .GroupBy(p => new
                {
                    style = p.Style,
                    color = p.Color
                })
                .Select(g => new
                {
                    style = g.Key.style,
                    color = g.Key.color
                })
                .OrderBy(sc => sc.style)
                .ThenBy(sc => sc.color)
                .GroupBy(sc => sc.style);

            foreach (var styleColor in allStyles)
            {
                StyleOption so = new StyleOption();
                so.Style = styleColor.Key;
                foreach (var item in styleColor)
                {
                    so.Colors.Add(item.color);
                }
                results.Styles.Add(so);
            }

            return results;
        }

        private ShoeDetailResponse createShoeDetailResponseFromShoeDetailInput(ShoeDetailInput input)
        {
            if (!String.IsNullOrEmpty(input.VNNumber))
            {
                var product = _context.Products.FirstOrDefault(p => p.VendorStyle == input.VNNumber);
                if (product == null)
                    throw new Exception(string.Format("Could not find product for VN Number: {0}", input.VNNumber));

                input.Style = product.Style;
                input.Color = product.Color;
            }

            List<Product> shoes = _context.Products
                .Where(s => s.Style == input.Style
                && s.Color == input.Color)
                .OrderBy(s => s.Size)
                .ToList();

            if (!shoes.Any())
                throw new Exception($"Could not find any products for Style: {input.Style} and Color: {input.Color}");
            
            var allSizes = _context.Sizes.OrderBy(s => s.USSize).ToList();
            ShoeDetailResponse response = new ShoeDetailResponse(input.Style, input.Color);

            // FILL MENS SHOES RESPONSE
            Product mensShoe = shoes.FirstOrDefault(s => s.Type == ShoeType.Men);
            if (mensShoe == null)
            {
                response.Mens.VNNumber = shoes.FirstOrDefault().VendorStyle;

                foreach (var size in allSizes.Where(s => s.Kids == false))
                {
                    response.Mens.AddShoe(size.USSize, 0);
                }
            }
            else
            {
                response.Mens.VNNumber = mensShoe.VendorStyle;

                var mensShoes = shoes
                        .Where(p => p.VendorStyle == mensShoe.VendorStyle
                        && p.Type == ShoeType.Men)
                        .OrderBy(p => p.Size)
                        .ToList();

                var inventory = _context.ProductLocations
                    .Where(i => mensShoes.Select(p => p.ProductId).Contains(i.ProductId));

                foreach (var size in allSizes.Where(s => s.Kids == false))
                {
                    var shoe = mensShoes.FirstOrDefault(s => s.Size == size.USSize);
                    if (shoe != null)
                    {
                        int quantity = inventory.Where(i => i.ProductId == shoe.ProductId).Sum(i => i.Quantity);
                        response.Mens.AddShoe(shoe.Size, quantity);
                    }
                    else
                    {
                        response.Mens.AddShoe(size.USSize, 0);
                    }
                }
            }

            // FILL WOMENS SHOES RESPONSE
            Product womensShoe = shoes.FirstOrDefault(s => s.Type == ShoeType.Women);
            if (womensShoe == null)
            {
                response.Womens.VNNumber = shoes.FirstOrDefault().VendorStyle;

                foreach (var size in allSizes.Where(s => s.Kids == false))
                {
                    float womensSize = ConvertMensToWomensSize(size.USSize);
                    response.Womens.AddShoe(womensSize, 0);
                }
            }
            else
            {
                response.Womens.VNNumber = womensShoe.VendorStyle;

                var womensShoes = shoes
                        .Where(p => p.VendorStyle == womensShoe.VendorStyle
                        && p.Type == ShoeType.Women)
                        .OrderBy(p => p.Size)
                        .ToList();

                var inventory = _context.ProductLocations
                    .Where(i => womensShoes.Select(p => p.ProductId).Contains(i.ProductId));

                foreach (var size in allSizes.Where(s => s.Kids == false))
                {
                    float womensSize = ConvertMensToWomensSize(size.USSize);
                    var shoe = womensShoes.FirstOrDefault(s => s.Size == size.USSize);

                    if (shoe != null)
                    {
                        int quantity = inventory.Where(i => i.ProductId == shoe.ProductId).Sum(i => i.Quantity);
                        response.Womens.AddShoe(womensSize, quantity);
                    }
                    else
                    {
                        response.Womens.AddShoe(womensSize, 0);
                    }
                }
            }

            // FILL KIDS SHOES RESPONSE
            Product kidsShoe = shoes.FirstOrDefault(s => s.Type == ShoeType.Youth);
            if (kidsShoe == null)
            {
                response.Youth.VNNumber = shoes.FirstOrDefault().VendorStyle;

                foreach (var size in allSizes.Where(s => s.Kids == true))
                {
                    response.Youth.AddShoe(size.USSize, 0);
                }
            }
            else
            {
                response.Youth.VNNumber = kidsShoe.VendorStyle;

                var kidsShoes = shoes
                        .Where(p => p.VendorStyle == kidsShoe.VendorStyle
                        && p.Type == ShoeType.Youth)
                        .OrderBy(p => p.Size)
                        .ToList();

                var inventory = _context.ProductLocations
                    .Where(i => kidsShoes.Select(p => p.ProductId).Contains(i.ProductId));

                foreach (var size in allSizes.Where(s => s.Kids == true))
                {
                    var shoe = kidsShoes.FirstOrDefault(s => s.Size == size.USSize);
                    if (shoe != null)
                    {
                        int quantity = inventory.Where(i => i.ProductId == shoe.ProductId).Sum(i => i.Quantity);
                        response.Youth.AddShoe(shoe.Size, quantity);
                    }
                    else
                    {
                        response.Youth.AddShoe(size.USSize, 0);
                    }
                }
            }
            
            return response;
        }

        private void fillShoeDetailResponseQuantities(ShoeDetailResponse response)
        {
            var sizes = _context.Sizes.OrderBy(s => s.SizeId).ToList();

            if (response.Mens != null)
            {
                var shoes = _context.Products
                        .Where(p => p.VendorStyle == response.Mens.VNNumber
                        && p.Type == ShoeType.Men)
                        .OrderBy(p => p.Size)
                        .ToList();

                var inventory = _context.ProductLocations
                    .Where(i => shoes.Select(p => p.ProductId).Contains(i.ProductId));

                foreach (var size in sizes.Where(s => s.Kids == false))
                {
                    var shoe = shoes.FirstOrDefault(s => s.Size == size.USSize);
                    if (shoe != null)
                    {
                        int quantity = inventory.Where(i => i.ProductId == shoe.ProductId).Sum(i => i.Quantity);
                        response.Mens.AddShoe(shoe.Size, quantity);
                    }
                    else
                    {
                        response.Mens.AddShoe(size.USSize, 0);
                    }
                }
            }


            if (response.Womens != null)
            {
                var shoes = _context.Products
                        .Where(p => p.VendorStyle == response.Womens.VNNumber
                        && p.Type == ShoeType.Women)
                        .OrderBy(p => p.Size)
                        .ToList();

                var inventory = _context.ProductLocations
                    .Where(i => shoes.Select(p => p.ProductId).Contains(i.ProductId));

                foreach (var size in sizes.Where(s => s.Kids == false))
                {
                    var shoe = shoes.FirstOrDefault(s => s.Size == size.USSize);
                    if (shoe != null)
                    {
                        int quantity = inventory.Where(i => i.ProductId == shoe.ProductId).Sum(i => i.Quantity);
                        float womensSize = ConvertMensToWomensSize(shoe.Size);
                        response.Womens.AddShoe(womensSize, quantity);
                    }
                    else
                    {
                        float womensSize = ConvertMensToWomensSize(shoe.Size);
                        response.Womens.AddShoe(size.USSize, 0);
                    }
                }


                foreach (var shoe in shoes)
                {
                    int quantity = inventory.Where(i => i.ProductId == shoe.ProductId).Sum(i => i.Quantity);
                    float size = ConvertMensToWomensSize(shoe.Size);
                    response.Womens.AddShoe(size, quantity);
                }
            }


            if (response.Youth != null)
            {
                var shoes = _context.Products
                        .Where(p => p.VendorStyle == response.Youth.VNNumber
                        && p.Type == ShoeType.Youth)
                        .OrderBy(p => p.Size)
                        .ToList();

                var inventory = _context.ProductLocations
                    .Where(i => shoes.Select(p => p.ProductId).Contains(i.ProductId));

                foreach (var shoe in shoes)
                {
                    int quantity = inventory.Where(i => i.ProductId == shoe.ProductId).Sum(i => i.Quantity);
                    response.Youth.AddShoe(shoe.Size, quantity);
                }
            }
        }
    }
}
