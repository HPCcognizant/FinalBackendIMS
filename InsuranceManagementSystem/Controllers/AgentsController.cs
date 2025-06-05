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
        [Authorize(Roles = "Admin, Agent")]
        public async Task<ActionResult<Agent>> PostAgent(AgentDTO agent)
        {
            string? userid = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            var createdAgent = await _agentService.AddAgentAsync(agent , userid);
            return CreatedAtAction(nameof(GetAgent), new { id = createdAgent.AgentID }, createdAgent);
        }

        // PUT: api/Agents/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> PutAgent(int id, AgentDTO agent)
        {
            try
            {
                var updatedAgent = await _agentService.UpdateAgentAsync(id, agent);
                return Ok(updatedAgent);
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid data");
            }
        }

        // DELETE: api/Agents/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            var success = await _agentService.DeleteAgentAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET: api/Agents/{agentId}/assignedPolicies
        [HttpGet("{agentId}/assignedPolicies")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Policy>>> GetAssignedPoliciesByAgentId(int agentId)
        {
            var policies = await _agentService.GetAssignedPoliciesByAgentIdAsync(agentId);
            return Ok(policies);
        }
    }
}