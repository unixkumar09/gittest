using Microsoft.AspNetCore.Mvc;
using Vans_SRMS_API.Models;
using Vans_SRMS_API.Repositories;
using Vans_SRMS_API.Database;

namespace Vans_SRMS_API.Controllers
{
    [Route("api/Store")]
    public class StoreController : Controller
    {
        private readonly SRMS_DbContext _context;
        private readonly IStoreRepository _storeRepo;

        public StoreController(SRMS_DbContext dbContext, IStoreRepository storeRepo)
        {
            _context = dbContext;
            _storeRepo = storeRepo;
        }

        /// <summary>
        /// Find the default store for this server
        /// </summary>
        /// <remarks>
        /// Use this route to find which store this server is in.  
        /// 
        /// Change the default store for this server by calling a PUT request to '/api/Store/{id}'
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Returns if store is found</response>
        /// <response code="404">Returns if no store is found for that ID</response>
        [HttpGet("Default")]
        public IActionResult Default()
        {
            Store store = _storeRepo.GetDefault();

            if (store == null)
                return NotFound("No stores found");

            return Ok(store);
        }

        /// <summary>
        /// Get details for a specific store
        /// </summary>
        /// <param name="id">Vans Store Number</param>
        /// <returns></returns>
        /// <response code="200">Returns if store is found</response>
        /// <response code="404">Returns if no store is found for that ID</response>
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            Store store = _storeRepo.Find(id.ToString());

            if (store == null)
                return NotFound(string.Format("Store {0} not found.", id));

            return Ok(store);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_storeRepo.GetAll());
        }

        /// <summary>
        /// Change the default store for this server
        /// </summary>
        /// <param name="id">Vans Store Number</param>
        /// <returns></returns>
        /// <response code="200">Returns if default store is set</response>
        /// <response code="404">Returns if no store is found for that ID</response>
        [HttpPut]
        public IActionResult SetStore(string storeNumber)
        {
            return _storeRepo.SetDefault(storeNumber).respond();
                    }
    }
}
