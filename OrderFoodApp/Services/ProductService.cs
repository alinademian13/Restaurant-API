﻿using Microsoft.EntityFrameworkCore;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderFoodApp.Services
{
    public interface IProductService
    {
        List<ProductGetModel> GetAllByCategoryId(int categoryId, User currentUser);

        Products GetById(int id, User currentUser);

        Products Create(ProductPostModel product, int categoryId, User employee);

        Products Update(int id, Products product, User employee);

        Products Delete(int id, User employee);
    }
    public class ProductService : IProductService
    {
        private RestaurantDbContext context;
        private ICategoryService categoryService;

        public ProductService(RestaurantDbContext context, ICategoryService categoryService)
        {
            this.context = context;
            this.categoryService = categoryService;
        }

        public List<ProductGetModel> GetAllByCategoryId(int categoryId, User currentUser)
        {
            var existingCategory = this.categoryService.GetById(categoryId, currentUser);

            if (existingCategory == null)
            {
                return null;
            }

            return context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => ProductGetModel.DtoFromModel(p)).ToList();
        }

        public Products GetById(int id, User currentUser)
        {
            var employee = context.Employees.FirstOrDefault(e => e.UserId == currentUser.Id);

            Products productById = context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);

            if (employee.RestaurantId != productById.Category.RestaurantId)
            {
                return null;
            }

            return productById;
        }

        public Products Create(ProductPostModel product, int categoryId, User employee)
        {
            Category category = categoryService.GetById(categoryId, employee);

            Products productToAdd = ProductPostModel.ToProduct(product);

            productToAdd.CategoryId = category.Id;

            context.Products.Add(productToAdd);

            context.SaveChanges();

            return productToAdd;
        }

        public Products Update(int id, Products product, User employee)
        {
            var existing = GetById(id, employee);

            if (existing ==  null)
            {
                return null;
            }

            product.Id = id;

            context.Products.Update(product);
            context.SaveChanges();

            return product;
        }

        public Products Delete(int id, User employee)
        {
            var existing = GetById(id, employee);

            if (existing == null)
            {
                return null;
            }

            context.Products.Remove(existing);
            context.SaveChanges();

            return existing;
        }
    }
}
