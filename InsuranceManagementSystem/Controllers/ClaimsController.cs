using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using InsuranceManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimServices _claimServices;
        public ClaimsController(IClaimServices claimServices)
        {
            _claimServices = claimServices;
        }
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAllClaimsFromDb()
        {
            var claims = await _claimServices.GetAllClaimsFromDb();
            return Ok(claims);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetClaimByIdFromDb(int id, [FromBody] ClaimDTO claimDto)
        {
            var claim = await _claimServices.GetClaimByIdFromDb(id);
            if (claim == null)
            {
                return NotFound();
            }
            return Ok(claim);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddClaimToDb([FromBody] ClaimDTO claimDto)
        {
            if (ModelState.IsValid)
            {
                string message = await _claimServices.AddClaimToDb(claimDto);
                return Ok(message);
            }
            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateClaimInDb(int id, string status)
        {
            if (ModelState.IsValid)
            {
                var updatedClaim = await _claimServices.UpdateClaimStatus(id, status);
                if (updatedClaim == null)
                {
                    return NotFound();
                }
                return Ok(updatedClaim);
            }
            return BadRequest(ModelState);


        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteClaimFromDb(int id)
        {
            await _claimServices.DeleteClaimFromDb(id);
            return Ok("Claim Deleted Successfully");
        }
       
    }
}