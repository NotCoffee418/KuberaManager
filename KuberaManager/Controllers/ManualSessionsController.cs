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

namespace KuberaManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManualSessionsController : Controller
    {
        private readonly kuberaDbContext _context;

        public ManualSessionsController(kuberaDbContext context)
        {
            _context = context;
        }

        // GET: ManualSessions
        public IActionResult Index()
        {
            return Redirect("/ManualSessions/create");
        }

        // GET: ManualSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManualSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,ComputerId,SelectedScenario,StopManuallly,RunUntil")] ManualSession manualSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manualSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manualSession);
        }
    }
}
