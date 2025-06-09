using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace InsuranceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _services;
        private readonly DatabaseDbContext _context;
        private readonly ITokenGenerate _tokenGenerate;

        public UsersController(IUserServices services, DatabaseDbContext context, ITokenGenerate tokenGenerate) 
        {
            _services = services;
            _context = context;
            _tokenGenerate = tokenGenerate;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _services.GetAllUser();
            return Ok(user);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterTheUser(UserDTO user)
        {
            if (user == null)
            {
                return BadRequest("Customer cannot be null");
            }
            try
            {
                await _services.RegisterOfUser(user);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                // Return a 400 Bad Request with the specific error message
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error for unexpected issues
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost("/registerAgent")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAgent(UserDTO user)
        {
            if (user == null)
            {
                return BadRequest("Customer cannot be null");
            }
            try
            {
                await _services.RegisterAgent(user);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                // Return a 400 Bad Request with the specific error message
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error for unexpected issues
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred: " + ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginOfUser([FromBody] LoginDTO login)
        {
            if(login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username and password cannot be empty");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                var token = _tokenGenerate.GenerateToken(user); 
                return Ok(new { token, user.role });
                
            }
            return Unauthorized("Invalid credentials");

        }
    }
}