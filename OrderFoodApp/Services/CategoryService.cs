using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Services
{
    public interface ICategoryService
    {
        Category GetById(int id, User currentUser);

        Category Create(CategoryPostModel category, User employee);

        Category Update(int id, Category category, User employee);

        Category ChangeStatus(int id, User employee);

        List<ProductGetModel> GetProductsForCategory(int categoryId, User employee);
    }

    public class CategoryService : ICategoryService
    {
        private RestaurantDbContext context;

        public CategoryService(RestaurantDbContext context)
        {
            this.context = context;
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
            if(existing == null)
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

        public List<ProductGetModel> GetProductsForCategory(int categoryId, User employee)
        {
            var existingCategory = GetById(categoryId, employee);

            if (existingCategory == null)
            {
                return null;
            }

            var result = context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => ProductGetModel.DtoFromModel(p)).ToList();

            return result;
        }

    }
}
