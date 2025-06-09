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

        public async Task<bool> RegisterOfUser(UserDTO user)
        {
            if (user == null)
                throw new ArgumentException("Enter valid User Details.");

            string HashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var UserInfo = new User
            {
                Username = user.Username,
                Password = HashPassword,
                Email = user.Email
            };

            try
            {
                // Check for existing username
                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                    throw new ArgumentException("Username already exists.");

                // Check for existing email
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                    throw new ArgumentException("Email already exists.");

                await _context.Users.AddAsync(UserInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new ArgumentException("Registration failed due to a database error: " + ex.Message);
            }
        }

        public async Task<bool> RegisterAgent(UserDTO user)
        {
            if (user == null)
                throw new ArgumentException("Enter valid User Details.");

            string HashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var UserInfo = new User
            {
                Username = user.Username,
                Password = HashPassword,
                Email = user.Email,
                role = "Agent"
            };

            try
            {
                // Check for existing username
                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                    throw new ArgumentException("Username already exists.");

                // Check for existing email
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                    throw new ArgumentException("Email already exists.");

                // Check for existing agent contact info
                if (await _context.Agents.AnyAsync(a => a.ContactInfo == user.Email)) // Adjust this if ContactInfo is not Email
                    throw new ArgumentException("Agent contact info already exists.");

                await _context.Users.AddAsync(UserInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new ArgumentException("Registration failed due to a database error: " + ex.Message);
            }
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
