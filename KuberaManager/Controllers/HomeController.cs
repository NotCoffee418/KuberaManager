using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KuberaManager.Models;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic;

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
                // Redirect to set admin password page if password isn't defined yet
                string adminPassHash = Config.Get<string>("AdminPassHash");
                if (adminPassHash == null || adminPassHash == "")
                    return Redirect("/config/changeadminpass");
            }
            catch (Exception ex)
            {
                ViewBag.Notification = ex.Message;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
