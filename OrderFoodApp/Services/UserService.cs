using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderFoodApp.DTO;
using OrderFoodApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderFoodApp.Services
{
    public interface IUsersService
    {
        LoginGetModel Authenticate(string email, string password);

        User Create(RegisterModel model, Role role);

        LoginGetModel Register(RegisterModel registerModel, Role role);

        User GetCurrentUser(HttpContext context);

        IEnumerable<LoginGetModel> GetAll();

        User GetUserById(int id);

        User UpdateUser(int id, User user);

        User DeleteUser(int id);
    }

    public class UserService : IUsersService
    {
        private RestaurantDbContext context;
        private readonly AppSettings appSettings;

        public UserService(RestaurantDbContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
        }

        public LoginGetModel Authenticate(string email, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Email == email &&
                                 x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new LoginGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Token = tokenHandler.WriteToken(token),
                UserRole = user.UserRole
            };
            // remove password before returning
            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public User Create(RegisterModel model, Role role)
        {
            User existing = context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (existing != null)
            {
                return null;
            }

            var user = new User
            {
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Password = ComputeSha256Hash(model.Password),
                UserRole = role,
            };

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public LoginGetModel Register(RegisterModel registerModel, Role role)
        {
            User user = Create(registerModel, role);

            if (user == null)
            {
                return null;
            }

            return Authenticate(registerModel.Email, registerModel.Password);
        }

        public IEnumerable<LoginGetModel> GetAll()
        {
            // return users without passwords
            return context.Users.Select(user => new LoginGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Token = null
            });
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string email = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            return context
                .Users
                .FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int id)
        {
            return context.Users.AsNoTracking()
                .FirstOrDefault(u => u.Id == id);
        }

        public User UpdateUser(int id, User user)
        {
            User userToBeUpdated = GetUserById(id);

            user.Id = id;

            user.Password = ComputeSha256Hash(userToBeUpdated.Password);
            context.Users.Update(user);
            context.SaveChanges();
            return user;

        }

        public User DeleteUser(int id)
        {
            var existing = context.Users.FirstOrDefault(user => user.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Users.Remove(existing);
            context.SaveChanges();

            return existing;
        }
    }
}
