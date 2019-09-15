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
    [Authorize(Roles = "Employee")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService categoryService;
        private IUsersService usersService;

        public CategoriesController(ICategoryService categoryService, IUsersService usersService)
        {
            this.categoryService = categoryService;
            this.usersService = usersService;
        }

        [HttpGet("restaurants/{restaurantId}/categories")]
        public IActionResult GetAllByRestaurantId(int restaurantId)
        {
            User currentUser = this.usersService.GetCurrentUser(HttpContext);

            return Ok(categoryService.GetAllByRestaurantId(restaurantId, currentUser));
        }

        [HttpGet("categories/{id}")]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var currentUser = usersService.GetCurrentUser(HttpContext);

            //var existing = this.categoryService.GetById(id, currentUser);

            var existing = this.categoryService.GetById(id);

            if (existing == null)
            {
                return NotFound();
            }
            return Ok(existing);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody]CategoryPostModel category)
        {
            User employee = usersService.GetCurrentUser(HttpContext);

            categoryService.Create(category, employee);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            var currentUser = usersService.GetCurrentUser(HttpContext);

            //var result = categoryService.Update(id, category, currentUser);

            var result = this.categoryService.Update(id, category);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}/change-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ChangeStatus(int id)
        {
            var currentUser = usersService.GetCurrentUser(HttpContext);

            //var result = categoryService.ChangeStatus(id, currentUser);

            var result = categoryService.ChangeStatus(id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}