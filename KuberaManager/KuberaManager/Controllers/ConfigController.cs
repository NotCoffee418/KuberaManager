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

        [HttpPost]
        public IActionResult Edit(Config config)
        {
            try
            {
                // handle unusual config keys
                switch (config.ConfKey)
                {
                    case "InstalledConfigVersion":
                        ViewBag.Notification = "InstalledConfigVersion cannot be manually updated. No changes were saved.";
                        break;

                    default: // Update value in db
                        Config.Set<string>(config.ConfKey, config.ConfValue);
                        break;
                }

                // Retrieve current values from DB
                ViewBag.ConfKey = config.ConfKey;
                ViewBag.ConfValue = Config.Get<string>(config.ConfKey);
            }
            catch (Exception ex)
            {
                ViewBag.Notification = "Failed to update config value because: " + ex.Message;
            }


            return View(config);
        }

        [HttpGet("Config/Edit/{confKey}")]
        public IActionResult Edit(string confKey)
        {
            string confValue = Config.Get<string>(confKey);
            Config newConf = new Config()
            {
                ConfKey = confKey,
                ConfValue = confValue
            };

            return View(newConf);
        }

    }
}
