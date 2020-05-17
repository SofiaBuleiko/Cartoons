using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CartoonsWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly CartoonsContext _context;

        public ChartsController(CartoonsContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            // var st  = _context.FilmStudios.ToList();
            var st = _context.FilmStudios.Include(l => l.Cartoons).ToList();
            List<object> stF = new List<object>();
            stF.Add(new[] { "Студія", "Кількість Мультфільмів" });
            foreach (var s in st)
            {
                stF.Add(new object[] { s.Name, s.Cartoons.Count() });
            }
            return new JsonResult(stF);
        }
        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
           var fst = _context.Cartoons.ToList();
           //var  fst = _context.
            List<object> cart = new List<object>();
            cart.Add(new[] { "Мультфільм", "Тривалість" });
            foreach (var g in fst)
            {
                cart.Add(new object[] { g.Name, g.Duration });
            }
            return new JsonResult(cart);
        }
    }
}
