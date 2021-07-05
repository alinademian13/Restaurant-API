using Microsoft.EntityFrameworkCore;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderFoodApp.Services
{
    public interface ICategoryService
    {
        List<CategoryGetModel> GetAllByRestaurantId(int restaurantId, User currentUser);

        Category GetById(int id, User currentUser);

        Category Create(CategoryPostModel category, User employee);

        Category Update(int id, Category category, User employee);

        Category ChangeStatus(int id, User employee);
    }

    public class CategoryService : ICategoryService
    {
        private RestaurantDbContext context;
        private IRestaurantService restaurantService;

        public CategoryService(RestaurantDbContext context, IRestaurantService restaurantService)
        {
            this.context = context;
            this.restaurantService = restaurantService;
        }

        public List<CategoryGetModel> GetAllByRestaurantId(int restaurantId, User currentUser)
        {
            var existingRestaurant = this.restaurantService.GetById(restaurantId);

            if (existingRestaurant == null)
            {
                return null;
            }

            var result = context.Categories.Where(c => c.RestaurantId == restaurantId);

            if (currentUser != null && currentUser.UserRole == Role.Employee)
            {
                var employeeRestaurantId = context.Employees.Where(e => e.UserId == currentUser.Id).Select(e => e.RestaurantId).FirstOrDefault();

                if (employeeRestaurantId != restaurantId)
                {
                    result = result.Where(c => c.IsActive == true);
                }
            }
            else
            {
                result = result.Where(c => c.IsActive == true);
            }

            return result
                .Select(c => new CategoryGetModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        IsActive = c.IsActive
                    })
                .ToList();
        }

        public Category GetById(int id, User currentUser)
        {
            Category category = context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (currentUser != null && currentUser.UserRole == Role.Employee)
            {
                int restaurantIdForEmployee = context
                        .Employees.FirstOrDefault(r => r.UserId == currentUser.Id).RestaurantId;

                if (category != null && category.RestaurantId != restaurantIdForEmployee)
                {
                    return null;
                }
            }  

            return category;
        }

        public Category Create(CategoryPostModel category, User employee)
        {
            int restaurantIdForEmployee = context
                .Employees.FirstOrDefault(r => r.UserId == employee.Id).RestaurantId;

            Category categoryToAdd = CategoryPostModel.ToCategory(category);
            categoryToAdd.RestaurantId = restaurantIdForEmployee;

            context.Categories.Add(categoryToAdd);
            context.SaveChanges();

            return categoryToAdd;
        }

        public Category Update(int id, Category category, User employee)
        {
            var existing = GetById(id, employee);

            if (existing == null)
            {
                return null;
            }

            category.Id = id;
            category.RestaurantId = existing.RestaurantId;
            category.IsActive = existing.IsActive;

            context.Categories.Update(category);
            context.SaveChanges();

            return category;
        }

        public Category ChangeStatus(int id, User employee)
        {
            var existing = GetById(id, employee);

            if (existing == null)
            {
                return null;
            }

            existing.IsActive = !existing.IsActive;

            context.Categories.Update(existing);
            context.SaveChanges();

            return existing;
        }
    }
}
