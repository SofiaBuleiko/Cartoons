using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CartoonsWebApp
{
    public partial class Genres
    {
        public Genres()
        {
            GenreCartoons = new HashSet<GenreCartoons>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Назва жанру")]
        public string Name { get; set; }

        public virtual ICollection<GenreCartoons> GenreCartoons { get; set; }
    }
}
