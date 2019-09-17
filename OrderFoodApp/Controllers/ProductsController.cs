using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using OrderFoodApp.Services;

namespace OrderFoodApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService productService;
        private IUserService userService;
        private ICategoryService categoryService;

        public ProductsController(IProductService productService, IUserService userService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.userService = userService;
            this.categoryService = categoryService;
        }

        [HttpGet("categories/{categoryId}/products")]
        public IActionResult GetAllByCategoryId(int categoryId)
        {
            User currentUser = this.userService.GetCurrentUser(HttpContext);

            return Ok(this.productService.GetAllByCategoryId(categoryId, currentUser));
        }

        [HttpGet("products/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var currentUser = this.userService.GetCurrentUser(HttpContext);

            var existing = this.productService.GetById(id, currentUser);

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
            User employee = this.userService.GetCurrentUser(HttpContext);

            productService.Create(product, employee);
        }

        [HttpPut("products/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(int id, [FromBody]Products product)
        {
            User currentUser = this.userService.GetCurrentUser(HttpContext);

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            User employee = this.userService.GetCurrentUser(HttpContext);

            var result = this.productService.Delete(id, employee);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}