using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Models.Services;

namespace SecretSanta.Controllers
{
    [Route("api/Santa")]
    [ApiController]
    public class SantaController : ControllerBase
    {
        // GET: api/Santa/SentGift/{UserCode}?v={true/false}
        [HttpGet("SentGift/{UserCode}")]
        public ActionResult SentGift(string UserCode, bool v, string key)
        {
            if (key != StringConstants.ApiKey)
            {
                return StatusCode(403, "Invalid key");
            }
            var santa = new SantasHelper();
            if (!santa.SetSantaSentGift(UserCode, v))
            {
                return BadRequest(santa.InfoMsg);
            }

            return Ok();
        }
    }
}
