using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CartoonsWebApp
{
    public partial class GenreCartoons
    {
        public int Id { get; set; }
        [Display(Name = "Мультфільм")]
        public int CartoonsId { get; set; }
        [Display(Name = "Жанр")]
        public int GenresId { get; set; }
        [Display(Name = "Мультфільм")]
        public virtual Cartoons Cartoons { get; set; }
        [Display(Name = "Жанр")]
        public virtual Genres Genres { get; set; }
    }
}
