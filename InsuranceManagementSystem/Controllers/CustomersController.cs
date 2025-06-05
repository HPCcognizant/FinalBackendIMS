using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerServices _services;
        private readonly ICustomerPolicyServices _cpServices;
        public CustomersController(ICustomerServices services, ICustomerPolicyServices cpServices)
        {
            _services = services;
            _cpServices = cpServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customer = await _services.GetAllCustomers();
            return Ok(customer);
        }


        [HttpGet("GetCustomerById")]
        public async Task<IActionResult> GetCustomerById()
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userid == null)
            {
                return BadRequest("Please log in to update your profile");
            }
            int id = Convert.ToInt32(userid);

            var customer = await _services.GetCustomerById(id);
            return Ok(customer);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCustomer(CustomerDTO customer)
        {
           
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            string ? email = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            
            if (userid == null && email == null ) 
            {
                return BadRequest("Unauthorize User");
            }

            if (customer == null)
            {
                return BadRequest("Customer cannot be null");
            }

            await _services.AddCustomer(customer, userid, email);
            return Ok(customer);
        }

        [HttpPut("UpdateTheProfile")]
        public async Task<IActionResult> UpdateCustomer(CustomerDTO customer)
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userid == null) {
                return BadRequest("Please log in to update your profile");
            }
            int id = Convert.ToInt32(userid);

            if (customer == null)
            {
                return BadRequest("Customer cannot be null");
            }

            await _services.UpdateCustomer(customer, id);

            return Ok("Customer Details Updated Successfully");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id) 
        {
            await _services.DeleteCustomer(id);
            return Ok("Customer Details Deleted Successfully");
        }

        [HttpGet("/IsProfileComplete")]
        [Authorize]
        public async Task<IActionResult> CheckingTheProfileCompletion()
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int id = Convert.ToInt32(userid);

            bool result = await _services.IsProfileCompleted(id);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return Ok(result);
            }
        }

    }
}
