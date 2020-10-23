using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KuberaManager.Models;
using KuberaManager.Models.Database;

namespace KuberaManager.Controllers
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
            // Validate database setup & config version
            try
            {
                // Redirect to config page on first run or DB update
                if (Config.Get<int>("InstalledConfigVersion") == 0)
                {
                    // Prepare Config table
                    Config.CheckUpdateConfigVersion();

                    // Return view
                    ViewBag.Notification = "New config values have been added. Please define them";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Notification = ex.Message;
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
