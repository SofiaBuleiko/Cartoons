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
    public class CartoonHeroesController : Controller
    {
        private readonly CartoonsContext _context;

        public CartoonHeroesController(CartoonsContext context)
        {
            _context = context;
        }

        // GET: CartoonHeroes
        public async Task<IActionResult> Index()
        {
            var cartoonsContext = _context.CartoonHeroes.Include(c => c.Cartoons);
            return View(await cartoonsContext.ToListAsync());
        }

        // GET: CartoonHeroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonHeroes = await _context.CartoonHeroes
                .Include(c => c.Cartoons)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoonHeroes == null)
            {
                return NotFound();
            }

            return View(cartoonHeroes);
        }

        // GET: CartoonHeroes/Create
        public IActionResult Create()
        {
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name");
            return View();
        }

        // POST: CartoonHeroes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CartoonsId,PeoplesId,Name,Discription")] CartoonHeroes cartoonHeroes)
        {
            int counter = 0;
            foreach (var a in _context.CartoonHeroes)
            {
                if (a.Name == cartoonHeroes.Name)
                { counter++; break; }
            }
            if (counter != 0)
            {
                ModelState.AddModelError("Name", "Така людина вже існує");
                return View(cartoonHeroes);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cartoonHeroes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", cartoonHeroes.CartoonsId);
                return View(cartoonHeroes);
            }
        }

        // GET: CartoonHeroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonHeroes = await _context.CartoonHeroes.FindAsync(id);
            if (cartoonHeroes == null)
            {
                return NotFound();
            }
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", cartoonHeroes.CartoonsId);
            return View(cartoonHeroes);
        }

        // POST: CartoonHeroes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CartoonsId,PeoplesId,Name,Discription")] CartoonHeroes cartoonHeroes)
        {
            if (id != cartoonHeroes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartoonHeroes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartoonHeroesExists(cartoonHeroes.Id))
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
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", cartoonHeroes.CartoonsId);
            return View(cartoonHeroes);
        }

        // GET: CartoonHeroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonHeroes = await _context.CartoonHeroes
                .Include(c => c.Cartoons)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoonHeroes == null)
            {
                return NotFound();
            }

            return View(cartoonHeroes);
        }

        // POST: CartoonHeroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartoonHeroes = await _context.CartoonHeroes.FindAsync(id);
            _context.CartoonHeroes.Remove(cartoonHeroes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartoonHeroesExists(int id)
        {
            return _context.CartoonHeroes.Any(e => e.Id == id);
        }
    }
}
