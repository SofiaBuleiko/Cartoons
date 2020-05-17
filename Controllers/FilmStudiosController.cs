using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CartoonsWebApp;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace CartoonsWebApp.Controllers
{
    public class FilmStudiosController : Controller
    {
        private readonly CartoonsContext _context;

        public FilmStudiosController(CartoonsContext context)
        {
            _context = context;
        }

        // GET: FilmStudios
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.filmStudiosName = name;
            var cartoonsContext = _context.FilmStudios.Include(f => f.Countries);
           
            return View(await cartoonsContext.ToListAsync());
        }

        // GET: FilmStudios/Details/5
        public async Task<IActionResult> Details(int? id, string? name)
        {
            ViewBag.filmStudiosName = name;
            if (id == null)
            {
                return NotFound();
            }
           
            var filmStudios = await _context.FilmStudios
               .Include(f => f. Cartoons)
                .Include(f => f.Countries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmStudios == null)
            {
                return NotFound();
            }
            //var bauth = from ba in _context.Cartoons
            //            where ba.FilmStudiosId == filmStudios.Id
            //            select ba;
            List<Cartoons> cartoons = new List<Cartoons>();

            //foreach (var ba in bauth)
            //{
            //    var auth = from a in _context.Cartoons
            //               where a.FilmStudios.Id == ba.FilmStudiosId
            //               select a;
            //    foreach (var au in auth)
            //    {
            //        cartoons.Add(au);
            //    }
            //}
            var auth = from ba in _context.Cartoons
                       where ba.FilmStudiosId == filmStudios.Id
                       select ba;
            foreach (var au in auth)
            {
                cartoons.Add(au);
            }
            ViewData["cartoonName"] = cartoons;

            return View(filmStudios);
        }

        // GET: FilmStudios/Create
        public IActionResult Create()
        {
            ViewData["CountriesId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: FilmStudios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountriesId")] FilmStudios filmStudios)
        {
            int counter = 0;
            foreach (var a in _context.FilmStudios)
            {
                if (a.Name == filmStudios.Name)
                { counter++; break; }
            }
            if (counter != 0)
            {
                ModelState.AddModelError("Name", "Така студія вже існує");
                return View(filmStudios);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(filmStudios);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CountriesId"] = new SelectList(_context.Countries, "Id", "Name", filmStudios.CountriesId);
                return View(filmStudios);
            }
        }

        // GET: FilmStudios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmStudios = await _context.FilmStudios.FindAsync(id);
            if (filmStudios == null)
            {
                return NotFound();
            }
            ViewData["CountriesId"] = new SelectList(_context.Countries, "Id", "Name", filmStudios.CountriesId);
            return View(filmStudios);
        }

        // POST: FilmStudios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountriesId")] FilmStudios filmStudios)
        {
            if (id != filmStudios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmStudios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmStudiosExists(filmStudios.Id))
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
            ViewData["CountriesId"] = new SelectList(_context.Countries, "Id", "Name", filmStudios.CountriesId);
            return View(filmStudios);
        }

        // GET: FilmStudios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmStudios = await _context.FilmStudios
                .Include(f => f.Countries)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmStudios == null)
            {
                return NotFound();
            }

            return View(filmStudios);
        }

        // POST: FilmStudios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filmStudios = await _context.FilmStudios.FindAsync(id);
            var cartoonfs = from ca in _context.Cartoons
                                where ca.FilmStudiosId == filmStudios.Id
                                select ca;
            foreach (var ca in cartoonfs)
            {
                _context.Cartoons.Remove(ca);
            }
            _context.FilmStudios.Remove(filmStudios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmStudiosExists(int id)
        {
            return _context.FilmStudios.Any(e => e.Id == id);
        }
       
    }
}
