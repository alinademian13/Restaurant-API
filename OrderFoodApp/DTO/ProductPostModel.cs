﻿using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class ProductPostModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }

        public static Products ToProduct(ProductPostModel product)
        {
            return new Products
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImagePath = product.ImagePath,
                CategoryId = product.CategoryId
            };
        }
    }
}
