﻿using System;
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
    public class RestaurantsController : ControllerBase
    {
        private IRestaurantService restaurantService;
        private IUsersService usersService;

        public RestaurantsController(IRestaurantService restaurantService, IUsersService usersService)
        {
            this.restaurantService = restaurantService;
            this.usersService = usersService;
        }

        [HttpGet]
        public PaginatedList<RestaurantGetModel> GetAll([FromQuery]int page = 1)
        {
            page = Math.Max(page, 1);
            return restaurantService.GetAll(page);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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
            var result = restaurantService.Update(id, restaurant);
            return Ok(result);
        }

        [HttpPut("{id}/change-status")]
        [Authorize(Roles = "Admin,Employee")]
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

        [HttpPost("{restaurantId}/new-employee")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddEmployee(int restaurantId, [FromBody]RegisterModel employee)
        {
            var existing = this.restaurantService.GetById(restaurantId);

            if (existing == null)
            {
                return NotFound();
            }

            var employeeToAdd = restaurantService.AddEmployee(restaurantId, employee);

            if (employeeToAdd == null)
            {
                return BadRequest(new { ErrorMessage = "Email address already exists." });
            }

            return Ok(employeeToAdd);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{restaurantId}/employees")]
        public IActionResult GetEmployeesForRestaurant(int restaurantId)
        {
            //List<User> employees = restaurantService.GetEmployeesForRestaurant(restaurantId);
            //return employees;
            return Ok(restaurantService.GetEmployeesForRestaurant(restaurantId));
        }

        [HttpGet("{restaurantId}/categories")]
        public IActionResult GetCategoriesForRestaurant(int restaurantId)
        {
            return Ok(restaurantService.GetCategoriesForRestaurant(restaurantId));
        }
    }
}