using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace KuberaManager.Controllers
{
    public class SetupController : Controller
    {
        public IActionResult Index()
        {
            if (System.IO.File.Exists("database.json"))
                return Forbid();

            else return Forbid();
        }
    }
}
