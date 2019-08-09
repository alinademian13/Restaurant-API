using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class CategoryGetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static CategoryGetModel DtoFromModel(Category category)
        {
            return new CategoryGetModel
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive
            };
        }
    }
}
