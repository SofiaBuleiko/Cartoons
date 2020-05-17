using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CartoonsWebApp;

namespace CartoonsWebApp.Controllers
{
    public class PeoplesController : Controller
    {
        private readonly CartoonsContext _context;

        public PeoplesController(CartoonsContext context)
        {
            _context = context;
        }

        // GET: Peoples
        public async Task<IActionResult> Index()
        {
            var cartoonsContext = _context.Peoples.Include(p => p.IdNavigation);
            return View(await cartoonsContext.ToListAsync());
        }

        // GET: Peoples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peoples = await _context.Peoples
                .Include(p => p.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peoples == null)
            {
                return NotFound();
            }

            return View(peoples);
        }

        // GET: Peoples/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.CartoonHeroes, "Id", "Name");
            return View();
        }

        // POST: Peoples/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Peoples peoples)
        {
            int counter = 0;
            foreach (var a in _context.Peoples)
            {
                if (a.Name == peoples.Name)
                { counter++; break; }
            }
            if (counter != 0)
            {
                ModelState.AddModelError("Name", "Така людина вже існує");
                return View(peoples);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(peoples);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["Id"] = new SelectList(_context.CartoonHeroes, "Id", "Name", peoples.Id);
                return View(peoples);
            }
        }

        // GET: Peoples/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peoples = await _context.Peoples.FindAsync(id);
            if (peoples == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.CartoonHeroes, "Id", "Name", peoples.Id);
            return View(peoples);
        }

        // POST: Peoples/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Peoples peoples)
        {
            if (id != peoples.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peoples);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeoplesExists(peoples.Id))
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
            ViewData["Id"] = new SelectList(_context.CartoonHeroes, "Id", "Name", peoples.Id);
            return View(peoples);
        }

        // GET: Peoples/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peoples = await _context.Peoples
                .Include(p => p.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (peoples == null)
            {
                return NotFound();
            }

            return View(peoples);
        }

        // POST: Peoples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peoples = await _context.Peoples.FindAsync(id);
            _context.Peoples.Remove(peoples);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeoplesExists(int id)
        {
            return _context.Peoples.Any(e => e.Id == id);
        }
    }
}
