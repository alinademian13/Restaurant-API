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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var currentUser = usersService.GetCurrentUser(HttpContext);

            var existing = this.categoryService.GetById(id, currentUser);
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

            var result = categoryService.Update(id, category, currentUser);

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

            var result = categoryService.ChangeStatus(id, currentUser);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        public IActionResult GetProductsForCategory(int categoryId)
        {
            var currentUser = usersService.GetCurrentUser(HttpContext);
            var existing = this.categoryService.GetById(categoryId, currentUser);
            if (existing == null)
            {
                return NotFound();
            }
            return Ok(categoryService.GetProductsForCategory(categoryId, currentUser));
        }

    }
}