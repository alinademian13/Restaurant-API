using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Models
{
    public class Products
    {
        [Key()]
        public int Id { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter the name of the product")]
        public string Name { get; set; }
        public int Price { get; set; }
        [Required(ErrorMessage = "Please enter the description of the product")]
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
