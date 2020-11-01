using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KuberaManager.Models.Database;
using KuberaManager.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Models.Logic;

namespace KuberaManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManualSessionsController : Controller
    {

        // GET: ManualSessions
        public IActionResult Index()
        {
            return Redirect("/ManualSessions/create");
        }

        // GET: ManualSessions/Create
        public IActionResult Create()
        {
            PrepareDisplayData();
            return View();
        }

        // POST: ManualSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,ComputerId,SelectedScenario,StopManually,RunUntil")] ManualSession ms)
        {
            if (ModelState.IsValid)
            {
                Brain.StartNewClient(ms.AccountId, ms.SelectedScenario, ms.ComputerId, ms.StopManually, ms.RunUntil);
                return Redirect("/");
            }

            PrepareDisplayData();
            return View(ms);
        }

        private void PrepareDisplayData()
        {
            // Init
            List<Account> accList = null;
            List<Computer> compList = null;
            List<ScenarioBase> scenList = null;

            // Get data
            using (var db = new kuberaDbContext())
            {
                // Get eligible accounts
                accList = db.Accounts
                    .Where(x => !x.IsBanned)
                    .Where(x => x.IsEnabled)
                    .ToList();

                // Get eligible computers
                compList = db.Computers
                    .ToList();

                // Get scenarios
                scenList = ScenarioHelper.AllScenarios;
            }

            // Accounts
            ViewBag.Accounts = new List<SelectListItem>();
            foreach (Account acc in accList)
                ViewBag.Accounts.Add(new SelectListItem { Value = acc.Id.ToString(), Text = acc.Login });

            // Computers
            ViewBag.Computers = new List<SelectListItem>();
            foreach (Computer comp in compList)
                ViewBag.Computers.Add(new SelectListItem { Value = comp.Id.ToString(), Text = comp.Hostname });

            // Scenarios
            ViewBag.Scenarios = new List<SelectListItem>();
            foreach (ScenarioBase scen in scenList)
                ViewBag.Scenarios.Add(new SelectListItem { Value = scen.Identifier, Text = scen.Identifier });
        }
    }
}
