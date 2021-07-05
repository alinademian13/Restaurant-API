using System;
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
    public class RestaurantsController : ControllerBase
    {
        private IRestaurantService restaurantService;
        private IUserService userService;

        public RestaurantsController(IRestaurantService restaurantService, IUserService userService)
        {
            this.restaurantService = restaurantService;
            this.userService = userService;
        }

        [HttpGet]
        public PaginatedList<RestaurantGetModel> GetAll([FromQuery]int page = 1)
        {
            page = Math.Max(page, 1);

            User currentUser = this.userService.GetCurrentUser(HttpContext);

            return restaurantService.GetAll(page, currentUser);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var existing = this.restaurantService.GetById(id);

            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody] RestaurantPostModel restaurant)
        {
            restaurantService.Create(restaurant);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Restaurant restaurant)
        {
            User currentUser = this.userService.GetCurrentUser(HttpContext);

            var result = restaurantService.Update(id, restaurant, currentUser);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{id}/change-status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ChangeStatus(int id)
        {
            var result = restaurantService.ChangeStatus(id);

            if (result ==  null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}