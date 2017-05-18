using System;
using System.Collections.Generic;
using System.Linq;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.ViewModels;
using Vans_SRMS_API.Database;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using static Vans_SRMS_API.Utils.Util;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Vans_SRMS_API.Repositories
{
    public interface IProductRepository
    {
        RepoResponse<string> Putaway(PutawayInput putaway, int deviceId);
        RepoResponse<string> Pick(PickInput pick, int deviceId);
        RepoResponse<List<ProductViewModel>> GetSome(int skip = 0, int take = 10);
        Task<RepoResponse<ItemMasterImportLog>> ImportAll();
        RepoResponse<int> Count();
    }

    public class ProductRepository : IProductRepository
    {
        private readonly SRMS_DbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(SRMS_DbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public class PutawayItem
        {
            public string GTIN { get; set; }
            public int Quantity { get; set; }
            public Product Product { get; set; }

            public PutawayItem(string g, int q)
            {
                GTIN = g;
                Quantity = q;
            }
        }


        public RepoResponse<string> Putaway(PutawayInput putaway, int deviceId)
        {
            DateTime now = DateTime.Now;
            Location location = _context.Locations
                .FirstOrDefault(l => l.Barcode == putaway.LocationBarcode);
            if (location == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest,
                    $"Barcode {putaway.LocationBarcode} is not a valid location");

            IEnumerable<PutawayItem> UniqueItemsToPutaway = putaway
                .GTINs
                .GroupBy(gtin => gtin)
                .Select(g => new PutawayItem(g.Key, g.Count()));

            foreach (PutawayItem item in UniqueItemsToPutaway)
            {
                Product product = _context.Products.FirstOrDefault(p => p.GTIN == item.GTIN);

                if (product == null)
                    return new RepoResponse<string>(HttpStatusCode.BadRequest,
                        $"GTIN: {item.GTIN} is not a valid product");

                item.Product = product;

                Putaway putawayLog = new Putaway(product.ProductId, location.LocationId, deviceId, now);
                _context.Putaways.Add(putawayLog);

                ProductLocation inventory = _context.ProductLocations
                    .FirstOrDefault(pl => pl.ProductId == item.Product.ProductId && pl.LocationId == location.LocationId);

                if (inventory == null)
                {
                    _context.ProductLocations.Add(new ProductLocation()
                    {
                        ProductId = item.Product.ProductId,
                        LocationId = location.LocationId,
                        Quantity = item.Quantity,
                        LastUpdatedAt = now,
                        LastUpdatedBy = deviceId
                    });
                }
                else
                {
                    inventory.Quantity += item.Quantity;
                    inventory.LastUpdatedAt = now;
                    inventory.LastUpdatedBy = deviceId;
                }

                product.InStock = true;
            }

            int changes = _context.SaveChanges();
            //todo: check response and rollback if needed

            return new RepoResponse<string>(HttpStatusCode.OK, "Putaway successful");
        }

        public RepoResponse<string> Pick(PickInput pick, int deviceId)
        {
            DateTime now = DateTime.Now;

            Location location = _context.Locations
                .FirstOrDefault(l => l.Barcode == pick.LocationBarcode);

            if (location == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest,
                    $"Barcode {pick.LocationBarcode} is not a valid location");

            Product product = _context.Products.FirstOrDefault(p => p.GTIN == pick.GTIN);

            if (product == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest,
                    $"GTIN: {pick.GTIN} is not a valid product");

            ProductLocation inventory = _context.ProductLocations
                .FirstOrDefault(i => i.LocationId == location.LocationId && i.ProductId == product.ProductId);

            if (inventory == null)
            {
                _logger.LogWarning("Attempt to Pick product from Location with Quantity=0", new { Product = product, Location = location, DeviceId = deviceId });
                location.FlaggedForCycleCount = true;
                location.FlaggedForAudit = true;

                inventory = new ProductLocation()
                {
                    LocationId = location.LocationId,
                    ProductId = product.ProductId,
                    Quantity = 0,
                    LastUpdatedAt = now,
                    LastUpdatedBy = deviceId
                };
                _context.ProductLocations.Add(inventory);
            }

            if (inventory.Quantity <= 1)
            {
                _context.ProductLocations.Remove(inventory);  // remove db table record instead of setting to 0
            }
            else
            {
                inventory.Quantity--;  // inventory can go into negative numbers if you pick from a location that has 0 quantity
                inventory.LastUpdatedAt = now;
                inventory.LastUpdatedBy = deviceId;
            }

            // update redundant product inStock flag
            var inStock = _context.ProductLocations.Where(pl => pl.ProductId == product.ProductId).Sum(p => p.Quantity);
            if (inStock == 0)
                product.InStock = false;

            RepoResponse<string> response = new RepoResponse<string>(HttpStatusCode.OK, "Pick successful");

            var order = getOrderForPick(pick, product, location);

            if (order != null)
            {
                order.Active = false;
                order.Picked = true;
                order.PickedAt = now;
                order.PickedBy = deviceId;
                order.LastUpdatedAt = now;
                order.LastUpdatedBy = deviceId;
                response.notes = response._BROADCAST_ORDER_UPDATES;
            }

            Pick newPick = new Pick()
            {
                DeviceId = deviceId,
                LocationId = location.LocationId,
                ProductId = product.ProductId,
                Order = order,
                Timestamp = now
            };
            _context.Picks.Add(newPick);

            int results = _context.SaveChanges();
            //todo: check response and rollback if needed
            return response;
        }


        public RepoResponse<List<ProductViewModel>> GetSome(int skip = 0, int take = 10)
        {
            return new RepoResponse<List<ProductViewModel>>(HttpStatusCode.OK,
                _context.Products.Skip(skip).Take(take).Select(p => p.toViewModel()).ToList());
        }

        public RepoResponse<int> Count()
        {
            return new RepoResponse<int>(HttpStatusCode.OK, _context.Products.Count());
        }

        public async Task<RepoResponse<ItemMasterImportLog>> ImportAll()
        {
            DateTime start = DateTime.Now;

            var file = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Data", "ItemMasterList.csv");
            if (!File.Exists(file))
                return new RepoResponse<ItemMasterImportLog>(HttpStatusCode.BadRequest,
                    $"Item Master List file not found. Expected file: {file.ToString()}");

            DateTime lastModified = File.GetLastWriteTimeUtc(file);
            ItemMasterImportLog lastImport = _context.ItemMasterImports
                .OrderByDescending(l => l.ItemMasterImportLogId)
                .FirstOrDefault();

            if (lastImport != null && lastImport.FileLastModifiedDate == lastModified.ToUniversalTime())
            {
                return new RepoResponse<ItemMasterImportLog>(HttpStatusCode.BadRequest,
                    $"Item Master List not updated.  File has not changed since the laset import. Last modified date: {lastModified.ToString()}.");
            }

            ItemMasterImportLog newLog = new ItemMasterImportLog()
            {
                FileLastModifiedDate = lastModified,
                StartedAt = DateTime.Now
            };
            _context.ItemMasterImports.Add(newLog);
            _context.SaveChanges();

            int parsed = 0;
            int updated = 0;
            int failed = 0;
            using (var sr = File.OpenText(file))
            {
                List<Product> productsParsed = new List<Product>();
                int count = 0;

                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    parsed++;

                    Product newProduct = await parseProductFromCSVAsync(line);
                    if (newProduct == null)
                    {
                        //TODO: log failed imports                     
                        failed++;
                        continue;
                    }

                    productsParsed.Add(newProduct);
                    count++;

                    if (count >= 250)
                    {
                        updated += await saveParsedProducts(productsParsed);
                        count = 0;
                        productsParsed.Clear();
                    }
                }

                updated += await saveParsedProducts(productsParsed);
            }

            DateTime finish = DateTime.Now;
            TimeSpan elapsed = finish.Subtract(start);

            newLog.FinishedAt = finish;
            newLog.ProductsImported = updated;
            newLog.ProductsFailedToImport = failed;
            newLog.RecordsInFile = parsed;

            _context.Entry(newLog).State = EntityState.Modified;
            _context.SaveChanges();

            return new RepoResponse<ItemMasterImportLog>(HttpStatusCode.OK, newLog);
        }


        private async Task<int> saveParsedProducts(List<Product> productsParsed)
        {
            List<string> uniqueGTINs = productsParsed.Select(pp => pp.GTIN).ToList();

            List<Product> productsToUpdate = _context.Products
                .Where(p => uniqueGTINs.Contains(p.GTIN))
                .ToList();

            List<string> existingGTINs = new List<string>();

            foreach (Product existingProduct in productsToUpdate)
            {
                existingGTINs.Add(existingProduct.GTIN);
                Product parsedProduct = productsParsed.First(pp => pp.GTIN == existingProduct.GTIN);

                if (!existingProduct.IsEqualTo(parsedProduct))
                {
                    existingProduct.Style = parsedProduct.Style;
                    existingProduct.Color = parsedProduct.Color;
                    existingProduct.Description = parsedProduct.Description;
                    existingProduct.VendorStyle = parsedProduct.VendorStyle;
                    existingProduct.ItemNumber = parsedProduct.ItemNumber;
                    existingProduct.Retail = parsedProduct.Retail;
                    existingProduct.Size = parsedProduct.Size;
                    existingProduct.Type = parsedProduct.Type;

                    _context.Entry(existingProduct).State = EntityState.Modified;
                }
            }

            int count = await _context.SaveChangesAsync();
            productsToUpdate.Clear();
            foreach (EntityEntry entry in _context.ChangeTracker.Entries().ToArray())
            {
                if (entry.Entity != null)
                    entry.State = EntityState.Detached;
            }

            IEnumerable<Product> productsToInsert = productsParsed
                .Where(pp => !existingGTINs.Contains(pp.GTIN));

            await _context.Products.AddRangeAsync(productsToInsert);
            count += await _context.SaveChangesAsync();

            productsToUpdate.Clear();
            foreach (EntityEntry entry in _context.ChangeTracker.Entries().ToArray())
            {
                if (entry.Entity != null)
                    entry.State = EntityState.Detached;
            }

            return count;
        }

        private async Task<Product> parseProductFromCSVAsync(string line)
        {
            var data = line.Split(new[] { ',' });
            /*
             * 
             * data[0] = Item Number
             * data[1] = VN Number
             * data[2] = GTIN
             * data[3] = Style
             * data[4] = Color
             * data[5] = Retail
             * data[6] = Description
             * data[7] = SKU
             * 
             */

            // VN Number
            if (data[1].Length < 12)
                return null;
            string vendorStyle = String.Format("VN{0}", data[1].Substring(0, 9));

            // Size
            float size = getSizeFromCode(data[1].Substring(9, 3));
            if (size == -1F)
                return null;

            // GTIN
            string GTIN = padGTIN(data[2]);

            // TYPE
            ShoeType? type = getShoeType(data[0].Substring(0, 2));
            if (type == null)
                return null;

            float convertedSize = (type.Value == ShoeType.Women) ? ConvertMensToWomensSize(size) : size;

            decimal retail = 0;

            decimal.TryParse(data[5], out retail);

            string style = data[3].Trim();
            string color = data[4].Trim();


            //int descLength = data[1].Length;
            //int descMiddle = descLength / 2;

            //var style = data[1].Substring(0, descMiddle);
            //var color = data[1].Substring(descMiddle, descLength - descMiddle);

            Product p = new Product()
            {
                ItemNumber = data[0],
                Description = data[6].Trim(),
                SKU = data[7],
                VendorStyle = vendorStyle,
                GTIN = GTIN,
                Style = style,
                Color = color,
                Size = size,
                ConvertedSize = convertedSize,
                Type = type.Value,
                Retail = retail
            };

            return p;
        }

        private Order getOrderForPick(PickInput input, Product product, Location location)
        {
            if (input.OrderId > 0)
            {
                return _context.Orders.FirstOrDefault(o => o.OrderId == input.OrderId && o.Active);
            }

            return _context.Orders
                .FirstOrDefault(o => o.VNNumber == product.VendorStyle
                && o.ConvertedSize == product.Size
                && o.SuggestedLocationId == location.LocationId
                && o.Active);
        }
    }
}
