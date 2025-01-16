using System.Diagnostics;
using envkeyweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace envkeyweb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            if (_env.IsDevelopment())
            {
                ViewBag.Message = "Running in Development";
            }
            else if (_env.IsProduction())
            {
                ViewBag.Message = "Running in Production";
            }

            return View();
        }

        public IActionResult Privacy()
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
