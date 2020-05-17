using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CartoonsWebApp
{
    public partial class CartoonAwards
    {
        public int Id { get; set; }
       
        [Display(Name = "Мультфільм")]
        public int CartoonsId { get; set; }
       
       [Display(Name = "Нагорода")]
        public int AwardsId { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Рік")]
        public int Year { get; set; }
     
        [Display(Name = "Нагорода")]
        public virtual Awards Awards { get; set; }
        
        [Display(Name = "Мультфільм")]
        public virtual Cartoons Cartoons { get; set; }
    }
}
