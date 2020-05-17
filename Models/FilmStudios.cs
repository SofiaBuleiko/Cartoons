using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CartoonsWebApp
{
    public partial class FilmStudios
    {
        public FilmStudios()
        {
            Cartoons = new HashSet<Cartoons>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Назва студії")]
        public string Name { get; set; }
        [Display(Name = "Назва студії")]
        //public string Cartoons { get; set; }
        //[Display(Name = "Мультфільми")]
        public int CountriesId { get; set; }

        [Display(Name = "Країна")]

        public virtual Countries Countries { get; set; }
        [Display(Name = "Мультфільми")]
        public virtual ICollection<Cartoons> Cartoons { get; set; }
    }
}
