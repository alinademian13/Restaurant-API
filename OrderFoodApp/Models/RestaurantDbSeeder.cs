using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Models
{
    public class RestaurantDbSeeder
    {
        public static void Initialize(RestaurantDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any flowers.
            if (context.Restaurants.Any())
            {
                return;   // DB has been seeded
            }

            context.SaveChanges();
        }
    }
}
