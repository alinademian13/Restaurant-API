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
    public class CategoriesController : ControllerBase
    {
        private ICategoryService categoryService;
        private IUserService userService;

        public CategoriesController(ICategoryService categoryService, IUserService userService)
        {
            this.categoryService = categoryService;
            this.userService = userService;
        }

        [HttpGet("restaurants/{restaurantId}/categories")]
        public IActionResult GetAllByRestaurantId(int restaurantId)
        {
            User currentUser = this.userService.GetCurrentUser(HttpContext);

            return Ok(categoryService.GetAllByRestaurantId(restaurantId, currentUser));
        }

        [HttpGet("categories/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var existing = this.categoryService.GetById(id);

            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
        }

        [HttpPost("categories")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody]CategoryPostModel category)
        {
            User employee = this.userService.GetCurrentUser(HttpContext);

            categoryService.Create(category, employee);
        }

        [HttpPut("categories/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            var result = this.categoryService.Update(id, category);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("categories/{id}/change-status")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ChangeStatus(int id)
        {
            var result = categoryService.ChangeStatus(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}