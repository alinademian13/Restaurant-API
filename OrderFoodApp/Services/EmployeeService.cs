using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrderFoodApp.Services
{
    public interface IEmployeeService
    {
        User Create(int restaurantId, RegisterModel employee);

        List<User> GetAllByRestaurantId(int restaurantId);
    }
    public class EmployeeService : IEmployeeService
    {
        private RestaurantDbContext context;
        private IRestaurantService restaurantService;
        private IUserService userService;

        public EmployeeService(RestaurantDbContext context, IRestaurantService restaurantService, IUserService userService)
        {
            this.context = context;
            this.restaurantService = restaurantService;
            this.userService = userService;
        }

        public User Create(int restaurantId, RegisterModel employee)
        {
            var existingRestaurant = this.restaurantService.GetById(restaurantId);

            if (existingRestaurant == null)
            {
                return null;
            }

            User employeeToAdd = this.userService.Create(employee, Role.Employee);

            if (employeeToAdd == null)
            {
                return null;
            }

            var newEmployee = new Employee
            {
                UserId = employeeToAdd.Id,
                RestaurantId = restaurantId
            };

            context.Employees.Add(newEmployee);
            context.SaveChanges();

            return employeeToAdd;
        }

        public List<User> GetAllByRestaurantId(int restaurantId)
        {
            var existingRestaurant = this.restaurantService.GetById(restaurantId);

            if (existingRestaurant == null)
            {
                return null;
            }

            var userIds = context.Employees.Where(e => e.RestaurantId == restaurantId).Select(e => e.UserId).ToList();

            return context.Users
                .Where(u => u.UserRole == Role.Employee && userIds.Contains(u.Id)).ToList();
        }
    }
}
