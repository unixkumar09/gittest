using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace Vans_SRMS_API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        // GET: api/values
        [HttpGet("EnvVar")]
        public string Get()
        {
            return Environment.GetEnvironmentVariable("SRMSConnection");
        }

        // GET api/values/5
        [HttpGet("EnvVar/{var}")]
        public string Get(string var)
        {
            return Environment.GetEnvironmentVariable(var);
        }
        
    }
}
