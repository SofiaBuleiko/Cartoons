using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CartoonsWebApp;
using Microsoft.AspNetCore.Authorization;

namespace CartoonsWebApp.Controllers
{
    public class CartoonsController : Controller
    {
        private readonly CartoonsContext _context;

        public CartoonsController(CartoonsContext context)
        {
            _context = context;
        }

        // GET: Cartoons
        public async Task<IActionResult> Index()
        {
            // if (id == null) return RedirectToAction("Awards", "Index");
            // ViewBag.AwardsId = id;
            // ViewBag.AwardsName = name;
            // var cartoonByAward = _context.CartoonAwards.Where(b => b.AwardsId ==id).Include(b => b.Awards);
            // ViewBag.FilmStudiosId = id;
           // ViewBag.CartoonsYear = year;
           var cartoonsContext = _context.Cartoons.Include(c => c.FilmStudios);
            return View(await cartoonsContext.ToListAsync());
        }
        //public async Task<IActionResult> Indexnew(int? id, string? name)
        //{
        //    // if (id == null) return RedirectToAction("Awards", "Index");
        //    // ViewBag.AwardsId = id;
        //    // ViewBag.AwardsName = name;
        //    // var cartoonByAward = _context.CartoonAwards.Where(b => b.AwardsId ==id).Include(b => b.Awards);
        //    ViewBag.FilmStudiosId = id;
        //    ViewBag.studioName = name;
        //    var cartoonsContext = _context.Cartoons.Include(c => c.FilmStudios);
        //    var cartoon = from car in _context.Cartoons
        //                         where car.FilmStudiosId == id
        //                         select car;
        //    List<Cartoons> cartoons = new List<Cartoons>();
        //    foreach(var c in cartoon)
        //    {
        //        cartoons.Add(c);
        //    }
        //    return View(await cartoon.ToListAsync());
        //}

        // GET: Cartoons/Details/5
        public async Task<IActionResult> Details(int? id, string? name)
        {
           ViewBag.cartoonName = name;
            ViewBag.CartoonsId = id;
           // int CartoonsYear = _context.Cartoons.Where(c => c.Id == ).Include(c => c.Year);
           // ViewData["CartoonsYear"] = year;
            if (id == null)
            {
                return NotFound();
            }

            var cartoons = await _context.Cartoons
                .Include(c => c.FilmStudios)
                  .FirstOrDefaultAsync(m => m.Id == id);
            //var awards = await _context.Cartoons
            //     .FirstOrDefaultAsync(m => m.Id == id);

            var cartoonawards = from ca in _context.CartoonAwards
                                where ca.CartoonsId == id
                                select ca;
           
            List<Awards> award = new List<Awards>();
           if (cartoonawards == null)
            {
             
             return NotFound(); }
            foreach (var ca in cartoonawards)
            {
                var aw = from g in _context.Awards
                         where g.Id == ca.AwardsId
                         select g;
                foreach (var gn in aw)
                { award.Add(gn);
                    
                }
                ViewData["CartoonAwardYear"] = ca.Year;
            }
            ViewData["AwardsName"] = award;
            if (award == null)
            {
                return NotFound();
            }

            return View(cartoons);
        }

        // GET: Cartoons/Create
        public IActionResult Create()
        {
            ViewData["FilmStudiosId"] = new SelectList(_context.FilmStudios, "Id", "Name");
            return View();
        }

        // POST: Cartoons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,Year,FilmStudiosId")] Cartoons cartoons)
        {
            int counter = 0;
            foreach (var a in _context.Cartoons)
            {
                if (a.Name == cartoons.Name)
                    
                { counter++; break; }
            }
            if (counter != 0)
            {
                ModelState.AddModelError("Name", "Такий мультик вже існує");
                ViewData["FilmStudiosId"] = new SelectList(_context.FilmStudios, "Id", "Name", cartoons.FilmStudiosId);
                return View(cartoons);
              

            }
            else
            {
                if (cartoons.Year < 1900 || cartoons.Year > 2020)
                {
                    ModelState.AddModelError("Year", "Некоректне значення");
                    ViewData["FilmStudiosId"] = new SelectList(_context.FilmStudios, "Id", "Name", cartoons.FilmStudiosId);
                    return View(cartoons);
                }
                else
                {
                    if (cartoons.Duration < 0)
                    {
                        ModelState.AddModelError("Duration", "Некоректне значення");
                        ViewData["FilmStudiosId"] = new SelectList(_context.FilmStudios, "Id", "Name", cartoons.FilmStudiosId);
                        return View(cartoons);
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            _context.Add(cartoons);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        ViewData["FilmStudiosId"] = new SelectList(_context.FilmStudios, "Id", "Name", cartoons.FilmStudiosId);
                        return View(cartoons);
                    }
                }
            }
        }

        // GET: Cartoons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoons = await _context.Cartoons.FindAsync(id);
            if (cartoons == null)
            {
                return NotFound();
            }
            ViewData["FilmStudiosId"] = new SelectList(_context.FilmStudios, "Id", "Name", cartoons.FilmStudiosId);
            return View(cartoons);
        }

        // POST: Cartoons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,Year,FilmStudiosId")] Cartoons cartoons)
        {
            if (id != cartoons.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartoons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartoonsExists(cartoons.Id))
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
            ViewData["FilmStudiosId"] = new SelectList(_context.FilmStudios, "Id", "Name", cartoons.FilmStudiosId);
            return View(cartoons);
        }

        // GET: Cartoons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoons = await _context.Cartoons
                .Include(c => c.FilmStudios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoons == null)
            {
                return NotFound();
            }

            return View(cartoons);
        }

        // POST: Cartoons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartoons = await _context.Cartoons.FindAsync(id);
            var cartoonawards = from ca in _context.CartoonAwards
                             where ca.CartoonsId == cartoons.Id
                             select ca;
            foreach (var ca in cartoonawards)
            {
                _context.CartoonAwards.Remove(ca);
            }
            var cartoonheroes = from ch in _context.CartoonHeroes
                              where ch.CartoonsId == cartoons.Id
                              select ch;
            foreach (var ch in cartoonheroes)
            {
                _context.CartoonHeroes.Remove(ch);
            }
            _context.Cartoons.Remove(cartoons);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartoonsExists(int id)
        {
            return _context.Cartoons.Any(e => e.Id == id);
        }
    }
}
