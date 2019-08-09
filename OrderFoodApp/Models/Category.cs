using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Models
{
    public class Category
    {
        [Key()]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the name of the category")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}
