﻿using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class LoginGetModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Role UserRole { get; set; }
        public int RestaurantId { get; set; }
    }
}
