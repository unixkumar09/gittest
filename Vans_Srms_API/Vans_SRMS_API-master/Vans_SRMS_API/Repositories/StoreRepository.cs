using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.Database;
using System.Collections.Generic;
using Vans_SRMS_API.ViewModels;

namespace Vans_SRMS_API.Repositories
{
    public interface IStoreRepository
    {
        Store Find(string storeNumber);
        Store GetDefault();
        RepoResponse<string> SetDefault(string storeNumber);
        List<Store> GetAll();
    }
    public class StoreRepository : IStoreRepository
    {
        private readonly SRMS_DbContext _context;

        public StoreRepository(SRMS_DbContext context)
        {
            _context = context;
        }

        public Store GetDefault()
        {
            return _context.Stores.FirstOrDefault(s => s.IsDefault);
        }

        public Store Find(string storeNumber)
        {
            return _context.Stores.FirstOrDefault(s => s.StoreNumber == storeNumber);
        }

        public List<Store> GetAll()
        {
            return _context.Stores.OrderBy(s => s.StoreNumber).ToList();
        }

        public RepoResponse<string> SetDefault(string storeNumber)
        {
            var store = _context.Stores.FirstOrDefault(s => s.StoreNumber == storeNumber);
            if (store == null)
                return new RepoResponse<string>(System.Net.HttpStatusCode.BadRequest, $"Store {storeNumber} does not exist in the system");

            // undo any existing default store settings
            var existingDefault = _context.Stores.Where(s => s.IsDefault).ToList();
            existingDefault.ForEach(s => s.IsDefault = false);
            
            // set current default store
            store.IsDefault = true;

            // update locations and devices with new store Id
            _context.Database.ExecuteSqlCommand(@"UPDATE ""SRMS"".""Locations"" SET ""StoreId"" = {0}", store.StoreId);
            _context.Database.ExecuteSqlCommand(@"UPDATE ""SRMS"".""StoreDevices"" SET ""StoreId"" = {0}", store.StoreId);

            _context.SaveChanges();

            return new RepoResponse<string>(System.Net.HttpStatusCode.OK, $"{storeNumber} set as the new default store");
        }
    }
}
