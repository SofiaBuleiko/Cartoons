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
    public class CartoonAwardsController : Controller
    {
        private readonly CartoonsContext _context;

        public CartoonAwardsController(CartoonsContext context)
        {
            _context = context;
        }

        // GET: CartoonAwards
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.cartoonsId = id;
            ViewBag.cartoonsName = name;
           
            var cartoonsContext = _context.CartoonAwards
                .Where(c => c.CartoonsId == id)
                .Include(c => c.Awards)
                .Include(c => c.Cartoons);
                

            return View(await cartoonsContext.ToListAsync());
        }
        //public async Task<IActionResult> Indexnew(int? id, string? name)
        //{
        //    ViewBag.cartoonsId = id;
        //    ViewBag.awardsName = name;
        //    var cartoonsContext = await _context.CartoonAwards
        //        .Include(c => c.Cartoons)
        //        .Include(c => c.Awards)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    var cartoon = from car in _context.CartoonAwards
        //                  where car.AwardsId == cartoonsContext.AwardsId
        //                  select car;
        //    List<Cartoons> cartoons = new List<Cartoons>();
        //    foreach (var c in cartoon)
        //    {
        //        var cart = from ca in _context.Cartoons
        //                   where ca.Id == c.CartoonsId
        //                   select ca;
        //        foreach (var car in cart)
        //        { cartoons.Add(car); }

        //    }
            
        //    return View(await cartoon.ToListAsync());
        //}

        // GET: CartoonAwards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonAwards = await _context.CartoonAwards
                .Include(c => c.Awards)
                .Include(c => c.Cartoons)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cartoonAwards == null)
            {
                return NotFound();
            }

            return View(cartoonAwards);
        }

        // GET: CartoonAwards/Create
        public IActionResult Create()
        {
            ViewData["AwardsId"] = new SelectList(_context.Awards, "Id", "Name");
           // ViewData["CartoonsId"] = id;
           ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name");
           // ViewBag.CartoonsYear = cart.Year;
           //int years = year;
            return View();
        }

        // POST: CartoonAwards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("CartoonsId,AwardsId,Year")] CartoonAwards cartoonAwards)
        {
           
            // var cartaw = await _context.Cartoons.Include(c => c.Year).FirstOrDefaultAsync();
            var cartoon = (from c in _context.Cartoons
                           where c.Id == cartoonAwards.CartoonsId
                           select c).FirstOrDefault();
            if (cartoon.Year > cartoonAwards.Year)
            {
                ModelState.AddModelError("Year", "Некоректне значення");
                return View(cartoonAwards);
            }
            else
            {
                if (ModelState.IsValid && cartoonAwards.Year > 0)
                {
                    _context.Add(cartoonAwards);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["AwardsId"] = new SelectList(_context.Awards, "Id", "Name", cartoonAwards.AwardsId);
                ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", cartoonAwards.CartoonsId);
               
                return View(cartoonAwards);
            }
        }

        // GET: CartoonAwards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonAwards = await _context.CartoonAwards.FindAsync(id);
            if (cartoonAwards == null)
            {
                return NotFound();
            }
            ViewData["AwardsId"] = new SelectList(_context.Awards, "Id", "Name", cartoonAwards.AwardsId);
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", cartoonAwards.CartoonsId);
            return View(cartoonAwards);
        }

        // POST: CartoonAwards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CartoonsId,AwardsId,Year")] CartoonAwards cartoonAwards)
        {
            if (id != cartoonAwards.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartoonAwards);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartoonAwardsExists(cartoonAwards.Id))
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
            ViewData["AwardsId"] = new SelectList(_context.Awards, "Id", "Name", cartoonAwards.AwardsId);
            ViewData["CartoonsId"] = new SelectList(_context.Cartoons, "Id", "Name", cartoonAwards.CartoonsId);
            return View(cartoonAwards);
        }

        // GET: CartoonAwards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonAwards = await _context.CartoonAwards
                .Include(c => c.Awards)
                .Include(c => c.Cartoons)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoonAwards == null)
            {
                return NotFound();
            }

            return View(cartoonAwards);
        }

        // POST: CartoonAwards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartoonAwards = await _context.CartoonAwards.FindAsync(id);
            _context.CartoonAwards.Remove(cartoonAwards);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartoonAwardsExists(int id)
        {
            return _context.CartoonAwards.Any(e => e.Id == id);
        }
    }
}
