using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
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
        [HttpPost]
        public async Task<IActionResult> AssignPolicy(CustomerPoliciesDTO customerPolicies)
        {
            string? email = User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;

            if (email == null)
            {
                return BadRequest("Unauthorized");
            }

            try
            {
                int custID = await _customerServices.FIndCustomerIdByEmail(email);

                var policiesDTO = new CustomerPoliciesDTO
                {
                    Customer_ID = custID,
                    PolicyID = customerPolicies.PolicyID,
                    StartDate = customerPolicies.StartDate,
                    PaymentFrequency = customerPolicies.PaymentFrequency,
                    PayableAmount = customerPolicies.PayableAmount,
                };

                var assignedPolicy = await _customerPolicyService.AssignPolicyToCustomerAsync(policiesDTO);
                return Ok(assignedPolicy);
            }
            catch (ArgumentException ex)
            {
                // Handles business logic errors, such as invalid frequency or already active policy
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handles unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred: " + ex.Message);
            }
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
        [HttpPut("UpdateTheRenewDate")]
        public async Task<IActionResult> UpdateTheRenewDate(int customerId, int policyId)
        {
            try
            {
                DateOnly renewDate = await _customerPolicyService.RenewPolicyAsync(customerId, policyId);
                return Ok(renewDate);
            }
            catch (ArgumentException ex)
            {
                // Handles cases like "Policy record not found for this customer."
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handles unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        [HttpGet("GetPayableAmount")]
        public async Task<IActionResult> GetPayabaleAmount(int policyId, string paymentFrequency)
        {
            try
            {
                decimal payableAmount = await _customerPolicyService.CalculatePayableAmountAsync(policyId, paymentFrequency);
                return Ok(payableAmount);
            }
            catch (ArgumentException ex)
            {
                // Handles cases like "Policy not found" or "Invalid payment frequency"
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handles unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred: " + ex.Message);
            }
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