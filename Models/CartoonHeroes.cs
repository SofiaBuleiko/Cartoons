using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CartoonsWebApp
{
    public partial class CartoonHeroes
    {
        public int Id { get; set; }
        [Display(Name = "Мультфільм")]
        public int CartoonsId { get; set; }
        [Display(Name = "Голос")]
        public int PeoplesId { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Ім'я героя")]
        public string Name { get; set; }
        
        [Display(Name = "Опис")]
        public string Discription { get; set; }
        [Display(Name = "Мультфільм")]

        public virtual Cartoons Cartoons { get; set; }
        [Display(Name = "Голос")]
        public virtual Peoples Peoples { get; set; }
    }
}
