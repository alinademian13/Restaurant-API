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

        public Products GetById(int id)
        {
            return context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
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
            throw new NotImplementedException();
        }

        public Products Delete(int id, User employee)
        {
            throw new NotImplementedException();
        }
    }
}
