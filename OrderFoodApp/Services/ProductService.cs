using Microsoft.EntityFrameworkCore;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApp.Services
{
    public interface IProductService
    {
        Products GetById(int id);
        List<ProductGetModel> GetAllByCategoryId(int categoryId);

        //Products GetById(int id, User currentUser);

        Products Create(ProductPostModel product);

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

        public Products GetById(int id)
        {
            return context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }
        public List<ProductGetModel> GetAllByCategoryId(int categoryId)
        {
            var existingCategory = this.categoryService.GetById(categoryId);

            if (existingCategory == null)
            {
                return null;
            }

            return context.Products
                .Where(p => p.CategoryId == categoryId)
                .Select(p => ProductGetModel.DtoFromModel(p)).ToList();
        }

        //public Products GetById(int id, User currentUser)
        //{
        //    var employee = context.Employees.FirstOrDefault(e => e.UserId == currentUser.Id);

        //    Products productById = context.Products
        //                            .AsNoTracking()
        //                            .FirstOrDefault(p => p.Id == id);

        //    Category categoryById = context.Categories
        //                            .AsNoTracking()
        //                            .FirstOrDefault(c => c.Id == productById.CategoryId);

        //    if (categoryById == null || employee.RestaurantId != categoryById.RestaurantId)
        //    {
        //        return null;
        //    }

        //    return productById;
        //}

        public Products Create(ProductPostModel product)
        {
            Products productToAdd = ProductPostModel.ToProduct(product);

            Category category = categoryService.GetById(productToAdd.CategoryId);

            productToAdd.CategoryId = category.Id;

            context.Products.Add(productToAdd);

            context.SaveChanges();

            return productToAdd;
        }

        public Products Update(int id, Products product, User employee)
        {
            var existing = GetById(id);

            if (existing == null)
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
            var existing = GetById(id);

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
