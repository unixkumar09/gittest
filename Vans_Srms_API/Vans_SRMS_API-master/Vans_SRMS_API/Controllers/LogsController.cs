using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vans_SRMS_API.Database;
using Vans_SRMS_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vans_SRMS_API.Controllers
{
    [Route("api/[controller]")]
    public class LogsController : Controller
    {
        private readonly SRMS_DbContext _context;

        public LogsController(SRMS_DbContext dbContext)
        {
             _context = dbContext;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Logs> Get(int skip = 0, int take = 20)
        {
            return _context.Logs
                .OrderByDescending(l=>l.Timestamp)
                .Skip(skip)
                .Take(take);
        }
        
    }
}
