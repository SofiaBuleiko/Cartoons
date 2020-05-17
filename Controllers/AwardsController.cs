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
    public class AwardsController : Controller
    {
        private readonly CartoonsContext _context;

        public AwardsController(CartoonsContext context)
        {
            _context = context;
        }

        // GET: Awards
        public async Task<IActionResult> Index( )
        {
            return View(await _context.Awards.ToListAsync());
        }

        
        // GET: Awards/Details/5
        public async Task<IActionResult> Details(int? id, string? name)
        {
            ViewBag. awardsName = name;
            ViewBag.awardsId = id;
            if (id == null)
            {
                return NotFound();
            }

            //var catoonAwards = await _context.CartoonAwards
            //    .Include(c => c.Cartoons)
            //    .Include(c => c.Awards)
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //var cartoon = from car in _context.CartoonAwards
            //              where car.CartoonsId == catoonAwards.CartoonsId
            //              select car;
            //List<Cartoons> cartoons = new List<Cartoons>();
            //foreach (var c in cartoon)
            //{
            //    var ca = from cart in _context.Cartoons
            //             where cart.Id == c.CartoonsId
            //             select cart;
            //    foreach(var carto in ca)
            //    {
            //        cartoons.Add(carto);
            //    }

            //}
            var awards = await _context.Awards
                  .FirstOrDefaultAsync(m => m.Id == id);

            var cartoonawards = from ca in _context.CartoonAwards
                            where ca.AwardsId == awards.Id
                            select ca;
            List<Cartoons> cartoon = new List<Cartoons>();
            foreach (var ca in cartoonawards)
            {
                var aw = from g in _context.Cartoons
                          where g.Id == ca.CartoonsId
                          select g;
                foreach (var gn in aw)
                { cartoon.Add(gn); }
            }
            ViewData["CartoonsName"] = cartoon;


           
            if (awards == null)
            {
                return NotFound();
            }

           //return RedirectToAction("Index", "Cartoons", new { id = awards.Id, name = awards.Name });
            return View(awards);
        }

        // GET: Awards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Awards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Awards awards)
        {
            
            int counter = 0;
            foreach(var a in _context.Awards)
            {
                if(a.Name == awards.Name)
                { counter++; break; }
            }
            if (counter != 0)
            {
                ModelState.AddModelError("Name", "Така нагорода вже існує");
                return View(awards);
            }
            else
            {
                
                if (ModelState.IsValid)
                {
                    _context.Add(awards);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(awards);
            }
        }

        // GET: Awards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awards = await _context.Awards.FindAsync(id);
            if (awards == null)
            {
                return NotFound();
            }
            return View(awards);
        }

        // POST: Awards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Awards awards)
        {
            if (id != awards.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(awards);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardsExists(awards.Id))
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
            return View(awards);
        }

        // GET: Awards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awards = await _context.Awards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (awards == null)
            {
                return NotFound();
            }

            return View(awards);
        }

        // POST: Awards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var awards = await _context.Awards.FindAsync(id);
            var cartoonawards = from ca in _context.CartoonAwards
                                where ca.AwardsId == awards.Id
                                select ca;
            foreach (var ca in cartoonawards)
            {
                _context.CartoonAwards.Remove(ca);
            }
            _context.Awards.Remove(awards);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AwardsExists(int id)
        {
            return _context.Awards.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            // var lang = _context.BookGenres.Include(b => b.Book).Include(b => b.Book.Language).ToList();
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Awards newaw;
                                var c = (from aw in _context.Awards
                                         where aw.Name.Contains(worksheet.Name)
                                         select aw).ToList();
                                if (c.Count > 0)
                                {
                                    newaw = c[0];
                                }
                                else
                                {
                                    newaw = new Awards();
                                    newaw.Name = worksheet.Name;
                                    _context.Awards.Add(newaw);
                                }
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try {  
                                    Cartoons cartoon = new Cartoons();
                                    int counter = 0;
                                    foreach (var ca in _context.Cartoons)
                                    {
                                        if (ca.Name == row.Cell(1).Value.ToString()) { counter++; cartoon = ca; break; }
                                    }
                                    if (counter > 0)
                                    {
                                        int count = 0;
                                        foreach (var award in _context.CartoonAwards)
                                        {
                                            if ((cartoon.Id == award.CartoonsId) && (newaw.Id == award.AwardsId)) { count++; break; }
                                        }
                                            if (count > 0)
                                            {
                                                goto link1;// якщо такф нагорода вже існує, переходимо до наступного рядка
                                            }
                                            else
                                            {
                                                CartoonAwards cartaw = new CartoonAwards();
                                                cartaw.Cartoons = cartoon;
                                                cartaw.Awards = newaw;
                                                try
                                                {
                                                    cartaw.Year = Convert.ToInt32(row.Cell(2).Value);
                                                    if (cartaw.Year <= cartoon.Year) { goto link1; }
                                                }
                                                catch { goto link1; }
                                            _context.CartoonAwards.Add(cartaw);
                                            goto link1;// переходимо до наступного рядка, бо вже маємо інформацію про нагороду цього мульта
                                        }

                                    }
                                    else
                                    {
                                        cartoon = new Cartoons();
                                        cartoon.Name = row.Cell(1).Value.ToString();

                                            try
                                            {
                                                cartoon.Year = Convert.ToInt32(row.Cell(3).Value);
                                                if(cartoon.Year <=0) { goto link1; }
                                                cartoon.Duration = Convert.ToInt32(row.Cell(4).Value);
                                            }
                                            catch { goto link1; }
                                        FilmStudios filmstudio = new FilmStudios();
                                        counter = 0;
                                        foreach (var fst in _context.FilmStudios)
                                        {
                                            if (fst.Name == row.Cell(5).Value.ToString()) { counter++; filmstudio = fst; break; }
                                        }
                                        if (counter > 0)
                                        {
                                            cartoon.FilmStudios = filmstudio;
                                        }
                                        else
                                        {
                                            Countries country = new Countries();
                                            counter = 0;
                                            foreach (var co in _context.Countries)
                                            {
                                                if (co.Name == row.Cell(6).Value.ToString()) { counter++; country = co; break; }
                                            }
                                            if (counter > 0)
                                            {
                                                filmstudio.CountriesId = country.Id;
                                            }
                                            else
                                            {
                                                country = new Countries();
                                                country.Name = row.Cell(6).Value.ToString();
                                                _context.Countries.Add(country);
                                                filmstudio.CountriesId = country.Id;
                                            }

                                            // BookGenres bg = new BookGenres();
                                            // cartoon.FilmStudios = filmstudio;
                                            filmstudio.Name = row.Cell(5).Value.ToString();
                                            _context.FilmStudios.Add(filmstudio);

                                        }
                                        _context.Cartoons.Add(cartoon);
                                        CartoonAwards cartaw = new CartoonAwards();
                                        cartaw.Cartoons = cartoon;
                                        cartaw.Awards = newaw;
                                            try
                                            {
                                                cartaw.Year = Convert.ToInt32(row.Cell(2).Value);
                                                if (cartaw.Year <= cartoon.Year) { goto link1; }
                                            }
                                            catch { goto link1; }
                                            _context.CartoonAwards.Add(cartaw);
                                    }
                                    
                                    }
                                   
                                    catch (Exception e)
                                    {
                                       
                                        throw new InvalidOperationException("Дані в файлі некоректні", e);
                                       // ModelState.AddModelError("", "Username or Password is wrong.");

                                        //logging самостійно :)

                                    }
                                   

                                    link1:;
                                    await _context.SaveChangesAsync();
                                }
                                //Genres newgen;
                                //var c = (from gen in _context.Genres
                                //         where gen.Name.Contains(worksheet.Name)
                                //         select gen).ToList();
                                //if (c.Count > 0)
                                //{
                                //    newgen = c[0];
                                //}
                                //else
                                //{
                                //    newgen = new Genres();
                                //    newgen.Name = worksheet.Name;
                                //    _context.Genres.Add(newgen);
                                //}
                                //перегляд усіх рядків                    

                            }
                        }
                    }
                }

            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Export(string name, int? id)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var awards = _context.CartoonAwards.Include(b => b.Cartoons).ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)

                var worksheet = workbook.Worksheets.Add(name);

                worksheet.Cell("A1").Value = "Назва";
                worksheet.Cell("B1").Value = "Рік нагороди";
                worksheet.Cell("C1").Value = "Рік випуску";
                worksheet.Cell("D1").Value = "Тривалість";
                    worksheet.Cell("E1").Value = "Студія";
                    worksheet.Cell("F1").Value = "Країна";
                   
                    worksheet.Row(1).Style.Font.Bold = true;
                var cartoonaw = from cartaw in _context.CartoonAwards
                                 where cartaw.AwardsId == id
                                 select cartaw;
                List<Cartoons> cartoons = new List<Cartoons>();
                foreach (var c in cartoonaw)
                {
                    var cart = from ca in _context.Cartoons
                               where ca.Id == c.CartoonsId
                               select ca;
                    foreach (var ca in cart)
                    { cartoons.Add(ca); }
                }

                //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                for (int i = 0; i < cartoons.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = cartoons[i].Name;
                    var caw = from c in _context.CartoonAwards
                              where c.CartoonsId == cartoons[i].Id
                              select c;
                    // var caw = _context.CartoonAwards.Where(a => a.CartoonsId == cartoons[i].Id).Include(a => a.Year).ToList();
                    foreach (var c in caw)
                    {
                        worksheet.Cell(i + 2, 2).Value = c.Year;
                    }

                    worksheet.Cell(i + 2, 3).Value = cartoons[i].Year;
                    worksheet.Cell(i + 2, 4).Value = cartoons[i].Duration;
                    var cfst = _context.FilmStudios.Where(a => a.Id == cartoons[i].FilmStudiosId).ToList();
                    foreach (var c in cfst)
                    {
                        worksheet.Cell(i + 2, 5).Value = c.Name;
                        var con = _context.Countries.Where(a => a.Id == c.CountriesId).ToList();
                        foreach (var cont in con) { worksheet.Cell(i + 2, 6).Value = cont.Name; }

                    }

                }
                
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"cartoons_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
