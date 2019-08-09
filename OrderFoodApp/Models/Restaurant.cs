using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Models
{
    public class Restaurant
    {
        [Key()]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the name of the restaurant")]
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        [StringLength(5, MinimumLength = 4, ErrorMessage = "Must be at least 4 and maximum 5 characters long")]
        public string OpeningTime { get; set; }
        [StringLength(5, MinimumLength = 4, ErrorMessage = "Must be at least 4 and maximum 5 characters long")]
        public string ClosingTime { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
