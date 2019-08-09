using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class RestaurantGetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public bool IsActive { get; set; }

        public static RestaurantGetModel DtoFromModel(Restaurant restaurant)
        {
            return new RestaurantGetModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Longitude = restaurant.Longitude,
                Latitude = restaurant.Latitude,
                OpeningTime = restaurant.OpeningTime,
                ClosingTime = restaurant.ClosingTime,
                IsActive = restaurant.IsActive
            };
        }
    }
}
