using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Models
{
    public enum Role
    {
        Regular, Employee, Admin
    }

    public class User
    {
        [Key()]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please write your Firstname")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please write your Lastname")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Please write your Email Address")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please write your Password")]
        public string Password { get; set; }
        [EnumDataType(typeof(Role))]
        public Role UserRole { get; set; }
    }
}
