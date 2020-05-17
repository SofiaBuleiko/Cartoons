using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CartoonsWebApp
{
    public partial class Peoples
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Голос ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Герой мультфільму")]
        public virtual CartoonHeroes IdNavigation { get; set; }
    }
}
