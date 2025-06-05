using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using InsuranceManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyServices _policyService;
        public PoliciesController(IPolicyServices policyService)
        {
            _policyService = policyService;
        }
        // GET: api/policy
        [HttpGet]
        //[Authorize(Roles = "Admin, User")] // Allow both Admin and User roles to access this endpoint
        public async Task<ActionResult<IEnumerable<Policy>>> GetPolicies()
        {
            var policies = await _policyService.GetAllAsync();
            return Ok(policies);
        }

        [HttpGet("/GetAllActivePolicies")]
        //[Authorize(Roles = "Admin, User")] // Allow both Admin and User roles to access this endpoint
        public async Task<ActionResult<IEnumerable<Policy>>> GetActivePolicies()
        {
            var policies = await _policyService.GetAllActivePoliciesAsync();
            return Ok(policies);
        }

        // GET: api/policy/{id}
        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Policy>> GetPolicyById(int id)
        {
            var policy = await _policyService.GetByIdAsync(id);
            if (policy == null)
            {
                return NotFound($"Policy with ID {id} not found.");
            }
            return Ok(policy);
        }

        // POST: api/policy
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<Policy>> CreatePolicy(PolicyDTO policyDto)
        {
            var newPolicy = await _policyService.AddAsync(policyDto);
            return CreatedAtAction(nameof(GetPolicyById), new { id = newPolicy.PolicyID }, newPolicy);
        }

        // PUT: api/policy/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Policy>> UpdatePolicy(int id, PolicyDTO policyDto)
        {
            var updatedPolicy = await _policyService.UpdateAsync(id, policyDto);
            if (updatedPolicy == null)
            {
                return NotFound($"Policy with ID {id} not found.");
            }
            return Ok(updatedPolicy);
        }

        // DELETE: api/policy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var existingPolicy = await _policyService.GetByIdAsync(id);
            if (existingPolicy == null)
            {
                return NotFound($"Policy with ID {id} not found.");
            }

            await _policyService.DeleteAsync(id);
            return NoContent(); // 204 No Content response after deletion
        }
    }
}