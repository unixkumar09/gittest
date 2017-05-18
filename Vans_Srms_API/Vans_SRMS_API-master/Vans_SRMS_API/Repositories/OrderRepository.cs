using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.ViewModels;
using static Vans_SRMS_API.Utils.Util;
using Vans_SRMS_API.Database;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Vans_SRMS_API.Repositories
{
    public interface IOrderRepository
    {
        RepoResponse<List<OrderResponse>> GetAll(bool ActiveOrders = true);
        RepoResponse<OrderResponse> GetOne(int id);
        Task<RepoResponse<string>> Create(OrderCreateInput input, int deviceId);
        Task<RepoResponse<string>> Delete(int id, int deviceId);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly SRMS_DbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(SRMS_DbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public RepoResponse<List<OrderResponse>> GetAll(bool ActiveOrders = true)
        {
            List<OrderResponse> response = new List<OrderResponse>();

            List<Order> orders = _context.Orders
                .Include(o => o.OrderedByDevice)
                .Where(o => o.Active == ActiveOrders)
                .OrderBy(o=>o.OrderedAt)
                .ToList();        

            List<StoreDevice> storeDevices = _context.StoreDevices
                .Include(sd => sd.Color)
                .Where(sd => orders.Select(o => o.OrderedBy).Distinct().Contains(sd.DeviceId) && sd.Active)
                .ToList();

            foreach (Order order in orders)
            {
                var color = storeDevices.FirstOrDefault(sd => sd.DeviceId == order.OrderedBy).Color.Hex;

                OrderResponse o = buildOrderResponse(order, color);
                if (o != null)
                    response.Add(o);
            }

            return new RepoResponse<List<OrderResponse>>(HttpStatusCode.OK, response);
        }

        public RepoResponse<OrderResponse> GetOne(int id)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            var color = _context.StoreDevices
                .Include(sd => sd.Color)
                .FirstOrDefault(sd => sd.DeviceId == order.OrderedBy && sd.Active)
                .Color.Hex;

            if (order == null)
                return new RepoResponse<OrderResponse>(HttpStatusCode.BadRequest, $"{ id } is not a valid Order ID");

            return new RepoResponse<OrderResponse>(HttpStatusCode.OK, buildOrderResponse(order, color));
        }

        public async Task<RepoResponse<string>> Create(OrderCreateInput input, int deviceId)
        {
            DateTime now = DateTime.Now;
            int storeId = getDefaultStore(_context);
            List<Order> newOrders = new List<Order>();

            foreach (var order in input.Orders)
            {
                float size = (order.Type == ShoeType.Women) ? ConvertWomensToMensSize(order.Size) : order.Size;

                Request request = _context.Requests.FirstOrDefault(r => r.RequestId == order.RequestId);
                if (request == null)
                    return new RepoResponse<string>(HttpStatusCode.BadRequest, $"{order.RequestId} is not a valid request ID");

                //todo: see if we need to use converted size
                // do womens shoes get imported with a mens size number? or is it already converted?
                Product product = _context.Products
                    .FirstOrDefault(p => p.VendorStyle == order.VNNumber
                    && p.Size == size
                    && p.Type == order.Type);

                if (product == null)
                    return new RepoResponse<string>(HttpStatusCode.BadRequest, $"{order.VNNumber} is not a valid product VN number");

                var productLocations = _context.ProductLocations
                    .Where(pl => pl.ProductId == product.ProductId);

                int quantity = productLocations.Sum(pl => pl.Quantity);

                if (quantity == 0)
                    return new RepoResponse<string>(HttpStatusCode.BadRequest, $"{ order.VNNumber } is currently out of stock in size {order.Size}");

                int preferredLocation = productLocations
                    .OrderByDescending(pl => pl.Quantity)
                    .First()
                    .LocationId;

                Order newOrder = new Order()
                {
                    RequestId = order.RequestId,
                    VNNumber = order.VNNumber,
                    Size = order.Size,
                    ConvertedSize = size,
                    Type = order.Type,
                    LastUpdatedAt = now,
                    LastUpdatedBy = deviceId,
                    OrderedAt = now,
                    OrderedBy = deviceId,
                    SuggestedLocationId = preferredLocation,
                    StoreId = storeId,
                    Active = true,
                    Picked = false
                };

                _context.Orders.Add(newOrder);
            }

            _context.SaveChanges();

            return new RepoResponse<string>(HttpStatusCode.OK, "Order Created");
        }

        public async Task<RepoResponse<string>> Delete(int id, int deviceId)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.OrderId == id && o.Active);

            if (order == null)
                return new RepoResponse<string>(HttpStatusCode.BadRequest, $"{ id } is not a valid Order ID");

            order.Active = false;
            order.LastUpdatedBy = deviceId;
            order.LastUpdatedAt = DateTime.Now;

            _context.SaveChanges();

            return new RepoResponse<string>(HttpStatusCode.OK, "Order Deleted");
        }


        private OrderResponse buildOrderResponse(Order order, string color)
        {
            float convertedSize = (order.Type == ShoeType.Women) ? ConvertWomensToMensSize(order.Size) : order.Size;

            Product shoe = _context.Products
                .FirstOrDefault(p => p.VendorStyle == order.VNNumber
                && p.Size == convertedSize
                && p.Type == order.Type);

            if (shoe == null)
                return null;

            IEnumerable<ProductLocation> productLocations = _context.ProductLocations
                .Include(i => i.Location)
                .Where(i => i.ProductId == shoe.ProductId && i.Quantity >= 1)
                .OrderByDescending(i => i.Quantity)
                .Take(6);

            List<LocationResponse> inventory = new List<LocationResponse>();
            if (productLocations.Any())
            {
                inventory = productLocations
                .Select(i => new LocationResponse()
                {
                    LocationId = i.LocationId,
                    LocationName = i.Location.Name,
                    Barcode = i.Location.Barcode,
                    Description = i.Location.Description
                })
                .ToList();
            }

            return new OrderResponse()
            {
                OrderId = order.OrderId,
                Hex = color,
                Style = shoe.Style,
                Color = shoe.Color,
                VNNumber = shoe.VendorStyle,
                GTIN = shoe.GTIN,
                Size = shoe.Size,
                Locations = inventory
            };

        }

    }
}
