using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Models.Services;

namespace SecretSanta.Controllers
{
    public class RandomizeController : Controller
    {
        public IActionResult Index()
        {
            var randomizer = new SantasHelper();
            var results = randomizer.RandomizeSecretSantas();
            return View(results);
        }
    }
}