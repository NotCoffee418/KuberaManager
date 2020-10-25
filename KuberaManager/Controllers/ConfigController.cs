using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using KuberaManager.Models.Database;
using KuberaManager.Models.Data;
using Microsoft.AspNetCore.Authorization;

namespace KuberaManager.Controllers
{
    [Authorize(Roles = "Admin")]
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

                    case "AdminPassHash":
                        ViewBag.Notification = "Admin password must be changed through the admin password page. No changes were saved";
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
            // Load current config value
            string confValue = "";
            
            // Handle exceptions
            switch (confKey)
            {
                case "AdminPassHash":
                    return Redirect("/Config/ChangeAdminPass");

                default:
                    Config.Get<string>(confKey);
                    break;

            }

            // Preload form
            Config newConf = new Config()
            {
                ConfKey = confKey,
                ConfValue = confValue
            };
            return View(newConf);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ChangeAdminPass()
        {
            // if pass isn't set or user is admin.
            string passHash = Config.Get<string>("AdminPassHash");
            if (passHash == null || passHash == "" || User.IsInRole("Admin"))
            {
                return View();
            }
            else return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ChangeAdminPass(AdminPassword adminPass)
        {
            // if pass isn't set or user is admin.
            string passHash = Config.Get<string>("AdminPassHash");
            if (passHash == null || passHash == "" || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Notification = "Admin password successfully updated";
                    Config.Set<string>("AdminPassHash", adminPass.GetPasswordHash());
                }

                return View();
            }
            else return Unauthorized();
        }
    }
}
