using Microsoft.EntityFrameworkCore;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Services
{
    public interface IRestaurantService
    {
        PaginatedList<RestaurantGetModel> GetAll(int page);

        Restaurant GetById(int id);

        Restaurant Create(RestaurantPostModel restaurant);

        Restaurant Update(int id, Restaurant restaurant);

        Restaurant ChangeStatus(int id);

        User AddEmployee(int restaurantId, RegisterModel employee);

        List<User> GetEmployeesForRestaurant(int restaurantId);

        List<CategoryGetModel> GetCategoriesForRestaurant(int restaurantId);
    }

    public class RestaurantService : IRestaurantService
    {
        private RestaurantDbContext context;
        private IUsersService usersService;
        public RestaurantService(RestaurantDbContext context, IUsersService usersService)
        {
            this.context = context;
            this.usersService = usersService;
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

        public PaginatedList<RestaurantGetModel> GetAll(int page)
        {
            IQueryable<Restaurant> result = context
                .Restaurants
                .OrderBy(r => r.IsActive ? 0 : 1);
                //.OrderBy(r => r.IsActive);
                //.Where(r => r.IsActive == true);

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

        public Restaurant Update(int id, Restaurant restaurant)
        {
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

        public User AddEmployee(int restaurantId, RegisterModel employee)
        {
            var existingRestaurant = GetById(restaurantId); 

            if (existingRestaurant == null)
            {
                return null;
            }

            User employeeToAdd = usersService.Create(employee, Role.Employee);

            if (employeeToAdd == null)
            {
                return null;
            }

            var newEmployee = new Employee
            {
                UserId = employeeToAdd.Id,
                RestaurantId = restaurantId
            };

            context.Employees.Add(newEmployee);
            context.SaveChanges();

            return employeeToAdd;
        }

        public List<User> GetEmployeesForRestaurant(int restaurantId)
        {
            var existingRestaurant = GetById(restaurantId);

            if (existingRestaurant == null)
            {
                return null;
            }

            var userIds = context.Employees.Where(e => e.RestaurantId == restaurantId).Select(e => e.UserId).ToList();

            return context.Users
                .Where(u => u.UserRole == Role.Employee && userIds.Contains(u.Id)).ToList();
        }

        public List<CategoryGetModel> GetCategoriesForRestaurant(int restaurantId)
        {
            var existingRestaurant = GetById(restaurantId);

            if (existingRestaurant == null)
            {
                return null;
            }

            var result = context.Categories
                .Where(c => c.IsActive == true && c.RestaurantId == restaurantId)
                .Select(c => new CategoryGetModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive
                });

            return result.ToList();
        }
    }
}
