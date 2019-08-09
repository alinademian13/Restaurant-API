using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class CategoryPostModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static Category ToCategory(CategoryPostModel category)
        {
            return new Category
            {
                Name = category.Name,
                IsActive = true
            };
        }
    }
}
