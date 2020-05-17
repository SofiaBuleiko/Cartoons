using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CartoonsWebApp
{
    public partial class Awards
    {
        public Awards()
        {
            CartoonAwards = new HashSet<CartoonAwards>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Мультфільми")]
        public virtual ICollection<CartoonAwards> CartoonAwards { get; set; }
    }
}
