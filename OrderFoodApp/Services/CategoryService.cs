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

            var result = context.Categories
                .Where(c => c.IsActive == true && c.RestaurantId == restaurantId)
                .Select(c => new CategoryGetModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive
                });

            if (currentUser.UserRole == Role.Regular)
            {
                result = result.Where(c => c.IsActive == true);
            }

            return result.ToList();
        }

        public Category GetById(int id, User currentUser)
        {
            var employee = context.Employees.FirstOrDefault(e => e.UserId == currentUser.Id);

            Category categoryById = context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);

            if (employee.RestaurantId != categoryById.RestaurantId)
            {
                return null;
            }

            return categoryById;
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
