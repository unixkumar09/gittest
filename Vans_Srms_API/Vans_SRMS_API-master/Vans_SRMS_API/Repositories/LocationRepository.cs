using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Vans_SRMS_API.Database;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.ViewModels;
using static Vans_SRMS_API.Utils.Util;

namespace Vans_SRMS_API.Repositories
{
    public interface ILocationRepository
    {
        RepoResponse<List<Location>> GetSome(int skip = 0, int take = 10);
        RepoResponse<string> Audit(CycleCountInput input, int deviceId);
        RepoResponse<string> CycleCount(CycleCountInput input, int deviceId);
        RepoResponse<LocationViewModel> Details(string barcode);
        RepoResponse<string> GenerateBackupFile();
    }
    public class LocationRepository : ILocationRepository
    {
        private readonly SRMS_DbContext _context;
        private readonly ILogger<LocationRepository> _logger;

        public LocationRepository(SRMS_DbContext context, ILogger<LocationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public class AuditItem
        {
            public string GTIN { get; set; }
            public int Quantity { get; set; }
            public Product Product { get; set; }

            public AuditItem(string g, int q)
            {
                GTIN = g;
                Quantity = q;
            }
        }


        public RepoResponse<List<Location>> GetSome(int skip = 0, int take = 10)
        {
            return new RepoResponse<List<Location>>(HttpStatusCode.OK, _context.Locations.Skip(skip).Take(take).ToList());
        }

        public RepoResponse<string> CycleCount(CycleCountInput input, int deviceId)
        {
            DateTime now = DateTime.Now;

            Location location = _context.Locations.FirstOrDefault(l => l.Barcode == input.LocationBarcode);
            if (location == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest, $"{input.LocationBarcode} is not a valid location");

            int storeId = getDefaultStore(_context);

            var productLocations = _context.ProductLocations
                .Where(pl => pl.LocationId == location.LocationId);
            _context.ProductLocations.RemoveRange(productLocations);

            CycleCount newCycleCount = new CycleCount()
            {
                DeviceId = deviceId,
                LocationId = location.LocationId,
                StoreId = storeId
            };
            _context.CycleCounts.Add(newCycleCount);

            IEnumerable<string> uniqueProductIds = input.Items.Select(i => i.GTIN).Distinct();
            IEnumerable<Product> products = _context.Products.Where(p => uniqueProductIds.Contains(p.GTIN));
            List<CycleCountItem> cycleCountItems = new List<CycleCountItem>();
            List<ProductLocation> newProductLocations = new List<ProductLocation>();

            foreach (var item in input.Items)
            {
                Product product = products.FirstOrDefault(p => p.GTIN == item.GTIN);
                if (product == null)
                    return new RepoResponse<string>(HttpStatusCode.BadRequest, $"GTIN {item.GTIN} is not a valid product");

                CycleCountItem cycleCountItem = new CycleCountItem()
                {
                    CycleCount = newCycleCount,
                    ProductId = product.ProductId,
                    Timestamp = item.ScannedAt
                };
                _context.CycleCountItems.Add(cycleCountItem);

                ProductLocation newProductLocation = newProductLocations
                    .FirstOrDefault(pl => pl.ProductId == product.ProductId);

                if (newProductLocation == null)
                {
                    newProductLocation = new ProductLocation()
                    {
                        LocationId = location.LocationId,
                        ProductId = product.ProductId,
                        Quantity = 1,
                        LastUpdatedAt = now,
                        LastUpdatedBy = deviceId
                    };
                    newProductLocations.Add(newProductLocation);
                }
                else
                {
                    newProductLocation.Quantity++;
                }

                // update redundant product inStock flag
                product.InStock = true;
            }

            _context.ProductLocations.AddRange(newProductLocations);
            int saveResult = _context.SaveChanges();

            //todo: check result count and rollback if needed

            return new RepoResponse<string>(HttpStatusCode.OK, "Cycle count created");
        }

        public RepoResponse<string> Audit(CycleCountInput input, int deviceId)
        {
            // DateTime now = DateTime.Now;

            Location location = _context.Locations.FirstOrDefault(l => l.Barcode == input.LocationBarcode);
            if (location == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest, $"{input.LocationBarcode} is not a valid location");

            int storeId = getDefaultStore(_context);

            LocationAudit newAudit = new LocationAudit()
            {
                StoreId = storeId,
                LocationId = location.LocationId,
                DeviceId = deviceId
            };
            _context.LocationAudits.Add(newAudit);

            var existingInventory = _context.ProductLocations
                .Where(pl => pl.LocationId == location.LocationId);

            IEnumerable<AuditItem> uniqueAuditItems = input
                .Items
                .GroupBy(i => i.GTIN)
                .Select(g => new AuditItem(g.Key, g.Count()));

            var scannedProducts = _context.Products
                .Where(p => uniqueAuditItems.Select(i => i.GTIN).Contains(p.GTIN))
                .ToList();

            foreach (var inputItem in uniqueAuditItems)
            {
                var product = scannedProducts.FirstOrDefault(p => p.GTIN == inputItem.GTIN);
                if (product == null)
                {
                    _logger.LogError("Location audit scanned an item that does not exist in the current product list", $"GTIN: {inputItem.GTIN}");
                    continue;
                }

                var exitingProductQuantity = existingInventory
                    .Where(i => i.ProductId == product.ProductId)
                    .Sum(i => i.Quantity);

                LocationAuditItem newAuditItem = new LocationAuditItem()
                {
                    LocationAudit = newAudit,
                    ProductId = product.ProductId,
                    ScannedQuantity = inputItem.Quantity,
                    OnRecordQuantity = exitingProductQuantity
                };
                _context.LocationAuditItems.Add(newAuditItem);
            }

            _context.SaveChanges();

            return new RepoResponse<string>(HttpStatusCode.OK, "Audit succcessful");
        }

        public RepoResponse<LocationViewModel> Details(string barcode)
        {
            Location location = _context.Locations.FirstOrDefault(l => l.Barcode == barcode);

            if (location == null)
                return new RepoResponse<LocationViewModel>(HttpStatusCode.BadRequest, $"{barcode} is not a valid location");

            var inventory = _context.ProductLocations
                .Include(pl => pl.Product)
                .Where(pl => pl.LocationId == location.LocationId)
                .ToList();

            var products = _context.ProductLocations
                .Include(pl => pl.Product)
                .Where(pl => pl.LocationId == location.LocationId)
                .Select(item => new LocationViewModel.LocationProducts()
                {
                    ProductId = item.ProductId,
                    GTIN = item.Product.GTIN,
                    VNNumber = item.Product.VendorStyle,
                    Style = item.Product.Style,
                    Color = item.Product.Color,
                    Size = item.Product.Size,
                    Type = item.Product.Type,
                    Quantity = item.Quantity
                })
                .ToList();

            LocationViewModel response = new LocationViewModel()
            {
                LocationName = location.Name,
                Barcode = location.Barcode,
                Description = location.Description,
                Products = products
            };

            return new RepoResponse<LocationViewModel>(HttpStatusCode.OK, response);
        }

        public RepoResponse<string> GenerateBackupFile()
        {
            var defaultStore = _context.Stores.FirstOrDefault(s => s.IsDefault);

            if (defaultStore == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest, "Default store not set");

            var inventory = _context.ProductLocations
                .Include(pl => pl.Product)
                .Include(pl => pl.Location)
                .Where(pl => pl.Quantity >= 1)
                .Select(i => new LocationBackupRecord()
                {
                    StoreNumber = defaultStore.StoreNumber,
                    LocationBarcode = i.Location.Barcode,
                    GTIN = i.Product.GTIN,
                    VendorStyle = i.Product.VendorStyle,
                    Style = i.Product.Style,
                    Color = i.Product.Color,
                    Size = i.Product.Size,
                    Quantity = i.Quantity
                })
                .ToList();

            if (!inventory.Any())
                return new RepoResponse<string>(HttpStatusCode.BadRequest, "0 products in stock");

            string outputFile = $"LocationBackup_Store-{defaultStore.StoreNumber}_{DateTime.Now.ToString("MM-dd-yyyy")}.csv";

            using (var file = File.Create(outputFile))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine(
                        string.Join<string>(",",
                            inventory.First().GetType().GetProperties().Select(p => p.Name)
                            )
                        );

                    foreach (var item in inventory)
                    {
                        var vals = item.GetType().GetProperties().Select(
                            pi => new
                            {
                                Value = pi.GetValue(item, null)
                            }
                        );

                        string line = string.Empty;

                        foreach (var val in vals)
                        {
                            if (val.Value == null)
                            {
                                line = string.Concat(string.Empty, ",");
                                continue;
                            }


                            var _val = val.Value.ToString();

                            //Check if the value contans a comma and place it in quotes if so
                            if (_val.Contains(","))
                                _val = string.Concat("\"", _val, "\"");

                            //Replace any \r or \n special characters from a new line with a space
                            if (_val.Contains("\r"))
                                _val = _val.Replace("\r", " ");
                            if (_val.Contains("\n"))
                                _val = _val.Replace("\n", " ");

                            line = string.Concat(line, _val, ",");

                        }

                        writer.WriteLine(line.TrimEnd(','));
                    }

                }
            }

            return new RepoResponse<string>(HttpStatusCode.OK, $"{outputFile} created successfull");

        }
    }
}
