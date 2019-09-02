using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFoodApp.DTO;
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

        [HttpGet("{id}")]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post([FromBody]ProductPostModel product)
        {

        }
    }
}