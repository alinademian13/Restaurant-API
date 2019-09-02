using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFoodApp.DTO;
using OrderFoodApp.Services;

namespace OrderFoodApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IRestaurantService restaurantService;
        private IEmployeeService employeeService;

        public EmployeesController(IRestaurantService restaurantService, IEmployeeService employeeService)
        {
            this.restaurantService = restaurantService;
            this.employeeService = employeeService;
        }

        [HttpGet("restaurants/{restaurantId}/employees")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllByRestaurantId(int restaurantId)
        {
            return Ok(employeeService.GetAllByRestaurantId(restaurantId));
        }

        [HttpPost("restaurants/{restaurantId}/employees")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(int restaurantId, [FromBody]RegisterModel employee)
        {
            var existing = this.restaurantService.GetById(restaurantId);

            if (existing == null)
            {
                return NotFound();
            }

            var employeeToAdd = this.employeeService.Create(restaurantId, employee);

            if (employeeToAdd == null)
            {
                return BadRequest(new { ErrorMessage = "Email address already exists." });
            }

            return Ok(employeeToAdd);
        } 
    }
}