using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using KuberaManager.Models.Database;

namespace KuberaManager.Controllers
{
    public class ConfigController : Controller
    {
        public IActionResult Index()
        {
            var context = new kuberaDbContext();
            ViewData.Model = context.Configs;
            return View();
        }

        public IActionResult Edit(string confKey)
        {
            var tehfuc = Config.Get<string>(confKey);
            return View(tehfuc);
        }
    }
}
