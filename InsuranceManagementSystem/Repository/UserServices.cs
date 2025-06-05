using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Repository
{
    public class UserServices : IUserServices
    {
        private readonly DatabaseDbContext _context;
        private readonly ITokenGenerate _tokenGenerate;
        public UserServices(DatabaseDbContext context, ITokenGenerate tokenGenerate) 
        {
            _context = context;
            _tokenGenerate = tokenGenerate;
        }

        //public async Task<string> LoginOfUser(string username, string password)
        //{
        //    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //    {
        //        throw new ArgumentException("Please enter a valid Username and Password.");
        //    }

        //    var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

        //    if (user == null)
        //    {
        //        throw new Exception("User Not Found.");
        //    }

        //    if (BCrypt.Net.BCrypt.Verify(password, user.Password))
        //    {
        //        return _tokenGenerate.GenerateToken(user);
        //    }

        //    throw new UnauthorizedAccessException("Invalid Credentials.");
        //}




        public async Task<bool> RegisterOfUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentException("Enter valid User Details.");
            }

            string HashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var UserInfo = new User
            {
                Username = user.Username,
                Password = HashPassword,
                Email = user.Email
            };

            await _context.Users.AddAsync(UserInfo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterAgent(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentException("Enter valid User Details.");
            }

            string HashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var UserInfo = new User
            {
                Username = user.Username,
                Password = HashPassword,
                Email = user.Email,
                role = "Agent"
            };

            await _context.Users.AddAsync(UserInfo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<User>> GetAllUser() 
        {
            return await _context.Users.Include(c=>c.Customer).Include(c=>c.Admin).Include(c=>c.Agent).ToListAsync();
        }

        public  Task CreateHashPassword(string password) 
        {
            string hashpass =  BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine(hashpass);

            return Task.FromResult(hashpass);
        }

    }
}
