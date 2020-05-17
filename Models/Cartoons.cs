using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CartoonsWebApp
{
    public partial class Cartoons
    {
        public Cartoons()
        {
            CartoonAwards = new HashSet<CartoonAwards>();
            CartoonHeroes = new HashSet<CartoonHeroes>();
            GenreCartoons = new HashSet<GenreCartoons>();
        }

        public int Id { get; set; }
        [Required (ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name ="Назва" )]

        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Тривалість")]
        public int Duration { get; set; }
            [Required(ErrorMessage = "Поле не може бути порожнім")]
            [Display(Name = "Рік випуску")]
        public int Year { get; set; }
        [Display(Name = "Студія")]
        public int FilmStudiosId { get; set; }
       // [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Студія")]
        public virtual FilmStudios FilmStudios { get; set; }
        [Display(Name = "Нагороди (якщо наявні):")]
        public virtual ICollection<CartoonAwards> CartoonAwards { get; set; }
        public virtual ICollection<CartoonHeroes> CartoonHeroes { get; set; }
        public virtual ICollection<GenreCartoons> GenreCartoons { get; set; }
    }
}
