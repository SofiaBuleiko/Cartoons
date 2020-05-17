using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CartoonsWebApp
{
    public partial class Countries
    {
        public Countries()
        {
            FilmStudios = new HashSet<FilmStudios>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Країна")]
        public string Name { get; set; }

        public virtual ICollection<FilmStudios> FilmStudios { get; set; }
    }
}
