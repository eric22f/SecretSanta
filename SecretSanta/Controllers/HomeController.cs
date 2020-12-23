using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta.Models;
using SecretSanta.Models.Services;

namespace SecretSanta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SecretSantaUser santa)
        {
            var getMySanta = new SantasHelper();

            var mySanta = getMySanta.GetSantasFromUserCode(santa.UserCode);
            if (mySanta == null)
            {
                santa.InfoMsg = getMySanta.InfoMsg;
                return View(santa);
            }
            mySanta.UserCode = santa.UserCode;
            mySanta.ApiKey = StringConstants.ApiKey;
            return View("MySecretSantaInfo", mySanta);
        }

        [HttpPost]
        public IActionResult MyInfo(MySecretSanta santa)
        {
            var getMyInfo = new SantasHelper();

            var mySanta = !string.IsNullOrWhiteSpace(santa.Address) ? getMyInfo.SaveMyInfo(santa) : getMyInfo.GeMyProfileFromUserCode(santa.UserCode);
            if (mySanta == null)
            {
                throw new Exception("Doh!");
            }
            mySanta.UserCode = santa.UserCode;
            return View(mySanta);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
