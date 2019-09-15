using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using OrderFoodApp.Services;

namespace OrderFoodApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService productService;
        private ICategoryService categoryService;
        private IUsersService usersService;

        public ProductsController(IProductService productService, ICategoryService categoryService, IUsersService usersService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.usersService = usersService;
        }

        [HttpGet("categories/{categoryId}/products")]
        public IActionResult GetAllByCategoryId(int categoryId)
        {
            return Ok(this.productService.GetAllByCategoryId(categoryId));
        }

        [HttpGet("products/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var existing = this.productService.GetById(id);
            if (existing == null)
            {
                return NotFound();
            }
            return Ok(existing);
        }

        [HttpPost("products")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody]ProductPostModel product)
        {
            productService.Create(product);
        }

        [HttpPut("products/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody]Products product)
        {
            User currentUser = this.usersService.GetCurrentUser(HttpContext);

            var result = this.productService.Update(id, product, currentUser);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("products/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete(int id)
        {
            User employee = this.usersService.GetCurrentUser(HttpContext);

            var result = this.productService.Delete(id, employee);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}