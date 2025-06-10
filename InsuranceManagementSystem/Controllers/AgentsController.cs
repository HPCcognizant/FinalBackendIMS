using Microsoft.AspNetCore.Mvc;
using InsuranceManagementSystem.Models;
using InsuranceManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using InsuranceManagementSystem.DTOs;

namespace InsuranceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentServices _agentService;

        public AgentsController(IAgentServices agentService)
        {
            _agentService = agentService;
        }

        // GET: api/Agents
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAgents()
        {
            var agents = await _agentService.GetAllAgentsAsync();
            return Ok(agents);
        }


        // GET: api/Agents/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Agent>> GetAgent(int id)
        {
            var agent = await _agentService.GetAgentByIdAsync(id);
            if (agent == null)
            {
                return NotFound();
            }
            return Ok(agent);
        }

        // POST: api/Agents
        [HttpPost]
        [Authorize(Roles = "Agent")]
        public async Task<ActionResult<Agent>> PostAgent(AgentDTO agent)
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            try
            {
                var createdAgent = await _agentService.AddAgentAsync(agent, userid);
                return CreatedAtAction(nameof(GetAgent), new { id = createdAgent.AgentID }, createdAgent);
            }
            catch (ArgumentException ex)
            {
                // Handles duplicate agent name or contact info
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handles unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred: " + ex.Message);
            }
        }


        // PUT: api/Agents/5
        [HttpPut("UpdateAgentProfile")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> PutAgent(AgentDTO agent)
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userid == null)
            {
                return BadRequest("Please log in to update your profile");
            }
            int id = Convert.ToInt32(userid);

            if (agent == null)
            {
                return BadRequest("Agent cannot be null");
            }

            await _agentService.UpdateAgentAsync(id , agent);

            return Ok("Agent Details Updated Successfully");

        }


        [HttpGet("GetAgentById")]
        [Authorize(Roles = "Agent, Admin")]
        public async Task<IActionResult> GetAgentById()
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userid == null)
            {
                return BadRequest("Please log in first");
            }
            int id = Convert.ToInt32(userid);

            var agent = await _agentService.GetAgentById(id);
            return Ok(agent);
        }


        // DELETE: api/Agents/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            try
            {
                var result = await _agentService.DeleteAgentAsync(id);
                if (result)
                {
                    return Ok("Agent deleted and policies reassigned successfully.");
                }
                else
                {
                    return NotFound("Agent not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Agents/{agentId}/assignedPolicies
        [HttpGet("{agentId}/assignedPolicies")]
        [Authorize(Roles = "Admin, User, Agent")]
        public async Task<ActionResult<IEnumerable<Policy>>> GetAssignedPoliciesByAgentId(string name)
        {
            var policies = await _agentService.GetAssignedPoliciesByAgentIdAsync(name);
            return Ok(policies);
        }

        [HttpGet("/IsAgentProfileComplete")]
        [Authorize]
        public async Task<IActionResult> CheckingTheProfileCompletion()
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int id = Convert.ToInt32(userid);

            bool result = await _agentService.IsProfileCompleted(id);
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