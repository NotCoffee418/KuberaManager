using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KuberaManager.Models.Database;
using Microsoft.AspNetCore.Authorization;

namespace KuberaManager.Controllers.Db
{
    [Authorize(Roles = "Admin")]
    public class ScenariosController : Controller
    {
        private readonly kuberaDbContext _context;

        public ScenariosController(kuberaDbContext context)
        {
            _context = context;
        }

        // GET: Scenarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Scenarios.ToListAsync());
        }

        // GET: Scenarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scenario = await _context.Scenarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scenario == null)
            {
                return NotFound();
            }

            return View(scenario);
        }

        // GET: Scenarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scenarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scenario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scenario);
        }

        // GET: Scenarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scenario = await _context.Scenarios.FindAsync(id);
            if (scenario == null)
            {
                return NotFound();
            }
            return View(scenario);
        }

        // POST: Scenarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Scenario scenario)
        {
            if (id != scenario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scenario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScenarioExists(scenario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(scenario);
        }

        // GET: Scenarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scenario = await _context.Scenarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scenario == null)
            {
                return NotFound();
            }

            return View(scenario);
        }

        // POST: Scenarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scenario = await _context.Scenarios.FindAsync(id);
            _context.Scenarios.Remove(scenario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScenarioExists(int id)
        {
            return _context.Scenarios.Any(e => e.Id == id);
        }
    }
}
