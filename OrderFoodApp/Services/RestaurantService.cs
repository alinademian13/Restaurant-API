using Microsoft.EntityFrameworkCore;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System.Linq;

namespace OrderFoodApp.Services
{
    public interface IRestaurantService
    {
        PaginatedList<RestaurantGetModel> GetAll(int page, User currentUser);

        Restaurant GetById(int id);

        Restaurant Create(RestaurantPostModel restaurant);

        Restaurant Update(int id, Restaurant restaurant, User currentUser);

        Restaurant ChangeStatus(int id);
    }

    public class RestaurantService : IRestaurantService
    {
        private RestaurantDbContext context;

        public RestaurantService(RestaurantDbContext context)
        {
            this.context = context;
        }

        public PaginatedList<RestaurantGetModel> GetAll(int page, User currentUser)
        {
            IQueryable<Restaurant> result = context
                .Restaurants
                .OrderBy(r => r.IsActive ? 0 : 1);

            if (currentUser == null || currentUser.UserRole != Role.Admin)
            {
                result = result.Where(r => r.IsActive == true);
            }

            PaginatedList<RestaurantGetModel> paginatedResult = new PaginatedList<RestaurantGetModel>();
            paginatedResult.CurrentPage = page;
            paginatedResult.NumberOfEntries = result.Count();
            paginatedResult.NumberOfPages = (paginatedResult.NumberOfEntries - 1) / PaginatedList<RestaurantGetModel>.EntriesPerPage + 1;

            result = result
                .Skip((page - 1) * PaginatedList<RestaurantGetModel>.EntriesPerPage)
                .Take(PaginatedList<RestaurantGetModel>.EntriesPerPage);

            paginatedResult.Entries = result.Select(r => RestaurantGetModel.DtoFromModel(r)).ToList();

            return paginatedResult;
        }

        public Restaurant GetById(int id)
        {
            return context.Restaurants.AsNoTracking().FirstOrDefault(r => r.Id == id);
        }

        public Restaurant Create(RestaurantPostModel restaurant)
        {
            Restaurant restaurantToAdd = RestaurantPostModel.ToRestaurant(restaurant);
            restaurantToAdd.IsActive = true;
            context.Restaurants.Add(restaurantToAdd);
            context.SaveChanges();
            return restaurantToAdd;
        }

        public Restaurant ChangeStatus(int id)
        {
            var existing = GetById(id);
            if (existing == null)
            {
                return null;
            }

            existing.IsActive = !existing.IsActive;
            context.Restaurants.Update(existing);
            context.SaveChanges();
            return existing;
        }

        public Restaurant Update(int id, Restaurant restaurant, User currentUser)
        {
            if (currentUser.UserRole == Role.Employee)
            {
                var restaurantId = context.Employees.Where(e => e.UserId == currentUser.Id).Select(e => e.RestaurantId).FirstOrDefault();

                if (restaurantId != id)
                {
                    return null;
                }
            }

            var existing = GetById(id);

            if (existing == null)
            {
                return null;
            }

            restaurant.Id = id;
            restaurant.IsActive = existing.IsActive;

            context.Restaurants.Update(restaurant);
            context.SaveChanges();

            return restaurant;
        }
    }
}
