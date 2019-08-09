using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.DTO
{
    public class RestaurantPostModel
    {
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }

        public static Restaurant ToRestaurant(RestaurantPostModel restaurant)
        {
            return new Restaurant
            {
                Name = restaurant.Name,
                Longitude = restaurant.Longitude,
                Latitude = restaurant.Latitude,
                OpeningTime = restaurant.OpeningTime,
                ClosingTime = restaurant.ClosingTime
            };
        }
    }
}
