using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class ProductGetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public static ProductGetModel DtoFromModel(Products product)
        {
            return new ProductGetModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImagePath = product.ImagePath
            };
        }
    }
}
