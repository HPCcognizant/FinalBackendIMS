using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPoliciesController : ControllerBase
    {
        private readonly ICustomerPolicyServices _customerPolicyService;
        private readonly ICustomerServices _customerServices;

        public CustomerPoliciesController(ICustomerPolicyServices customerPolicyService, ICustomerServices customerServices)
        {
            _customerPolicyService = customerPolicyService;
            _customerServices = customerServices;
        }

        // Assign Policy to Customer
        [HttpPost("assign")]
        public async Task<IActionResult> AssignPolicy(int policyId)
        {
            string? email = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            


            if (email == null) 
            {
                return BadRequest("Unauthorized");
            }


            int custID = await _customerServices.FIndCustomerIdByEmail(email);

            var policiesDTO = new CustomerPoliciesDTO
            {
                Customer_ID = custID,
                PolicyID = policyId,
            };

            var assignedPolicy = await _customerPolicyService.AssignPolicyToCustomerAsync(policiesDTO);
            return Ok(assignedPolicy);
        }

        // Get All Assigned Policies
        [HttpGet]
        public async Task<IActionResult> GetAllAssignedPolicies()
        {
            var policies = await _customerPolicyService.GetAllAsync();
            return Ok(policies);
        }

        // Get Assigned Policy by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignedPolicyById(int id)
        {
            var policy = await _customerPolicyService.GetByIdAsync(id);
            if (policy == null) return NotFound();
            return Ok(policy);
        }

        // Update Assigned Policy
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignedPolicy(int id, [FromBody] CustomerPoliciesDTO policiesDTO)
        {
            var updatedPolicy = await _customerPolicyService.UpdateAssignedPolicyAsync(id, policiesDTO);
            if (updatedPolicy == null) return NotFound();
            return Ok(updatedPolicy);
        }

        // Delete Assigned Policy
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignedPolicy(int id)
        {
            await _customerPolicyService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("/GetAllPoliciesByCustomerId")]
        public async Task<IActionResult> GetAllThePoliciesByCustomerID(int id)
        {
            var policy = await _customerPolicyService.GetAllPoliciesByCustomerID(id);
            if (policy == null) return NotFound("No Policies were found for Customer");

            return Ok(policy);
        }

        [HttpGet("/GetAllCustomersByPolicyId")]
        public async Task<IActionResult> GetAllTheCustomersByPolicyID(int id)
        {
            var policy = await _customerPolicyService.GetAllCustomerByPolicyID(id);
            if (policy == null) return NotFound("No Customer were found for Policy");

            return Ok(policy);
        }
    }
}