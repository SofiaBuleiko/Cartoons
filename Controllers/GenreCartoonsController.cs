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
    public class GenreCartoonsController : Controller
    {
        private readonly CartoonsContext _context;

        public GenreCartoonsController(CartoonsContext context)
        {
            _context = context;
        }

        // GET: GenreCartoons
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.genreId = id;
            ViewBag.genreName = name;
            var cartoonByGenre = _context.GenreCartoons.Where(b => b.GenresId == id).Include(b => b.Genres);

            // var cartoonsContext = _context.GenreCartoons.Include(g => g.Cartoons).Include(g => g.Genres);
            return View(await cartoonByGenre.ToListAsync());
        }

        // GET: GenreCartoons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreCartoons = await _context.GenreCartoons
                .Include(g => g.Cartoons)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genreCartoons == null)
            {
                return NotFound();
            }

            return View(genreCartoons);
        }

        // GET: GenreCartoons/Create
        public IActionResult Create(int? id, string? name)
        {
            ViewBag.genreId = id;
            ViewBag.genreName = name;
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name");
            return View();
        }

        // POST: GenreCartoons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, string name, [Bind("Id,CartoonsId")] GenreCartoons genreCartoons)
        {
            GenreCartoons tg = new GenreCartoons();
            tg.GenresId = id;
            genreCartoons.GenresId = id;
            tg.CartoonsId = genreCartoons.CartoonsId;
            genreCartoons.GenresId = id;
            if (ModelState.IsValid)
            {
                _context.Add(genreCartoons);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id, name});
            }
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", genreCartoons.CartoonsId);
            return RedirectToAction("Index", new { id, name });
        }

        // GET: GenreCartoons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreCartoons = await _context.GenreCartoons.FindAsync(id);
            if (genreCartoons == null)
            {
                return NotFound();
            }
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", genreCartoons.CartoonsId);
            ViewData["GenresId"] = new SelectList(_context.Genres, "Id", "Name", genreCartoons.GenresId);
            return View(genreCartoons);
        }

        // POST: GenreCartoons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CartoonsId,GenresId")] GenreCartoons genreCartoons)
        {
            if (id != genreCartoons.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genreCartoons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreCartoonsExists(genreCartoons.Id))
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
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", genreCartoons.CartoonsId);
            ViewData["GenresId"] = new SelectList(_context.Genres, "Id", "Name", genreCartoons.GenresId);
            return View(genreCartoons);
        }

        // GET: GenreCartoons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genreCartoons = await _context.GenreCartoons
                .Include(g => g.Cartoons)
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genreCartoons == null)
            {
                return NotFound();
            }

            return View(genreCartoons);
        }

        // POST: GenreCartoons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genreCartoons = await _context.GenreCartoons.FindAsync(id);
            _context.GenreCartoons.Remove(genreCartoons);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreCartoonsExists(int id)
        {
            return _context.GenreCartoons.Any(e => e.Id == id);
        }
    }
}
