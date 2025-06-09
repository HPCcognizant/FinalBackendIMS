using Microsoft.EntityFrameworkCore;
using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.Models;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.DTOs;

namespace InsuranceManagementSystem.Services
{
    public class AgentServices : IAgentServices
    {
        private readonly DatabaseDbContext _context;

        public AgentServices(DatabaseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            return await _context.Agents.Include(a => a.AssignedPolicies).ToListAsync();
        }

        public async Task<Agent> GetAgentByIdAsync(int id)
        {
            return await _context.Agents.Include(a => a.AssignedPolicies)
                .FirstOrDefaultAsync(a => a.AgentID == id);
        }

        public async Task<Agent> AddAgentAsync(AgentDTO agent, string userid)
        {
            int uid = Convert.ToInt32(userid);

            // Check for existing agent name
            var existingName = await _context.Agents.FirstOrDefaultAsync(x => x.Agent_Name == agent.Agent_Name);
            if (existingName != null)
            {
                throw new ArgumentException("Agent name already exists. Please choose a different name.");
            }

            // Check for existing contact info
            var existingContact = await _context.Agents.FirstOrDefaultAsync(x => x.ContactInfo == agent.ContactInfo);
            if (existingContact != null)
            {
                throw new ArgumentException("Contact info already exists. Please provide a different contact number.");
            }

            var agentInfo = new Agent
            {
                Agent_Name = agent.Agent_Name,
                ContactInfo = agent.ContactInfo,
                UserId = uid,
            };

            try
            {
                _context.Agents.Add(agentInfo);
                await _context.SaveChangesAsync();
                return agentInfo;
            }
            catch (DbUpdateException ex)
            {
                // Fallback for any unique constraint violation at the database level
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                {
                    throw new ArgumentException("contact info already exists.");
                }
                throw;
            }
        }


        public async Task UpdateAgentAsync(int id, AgentDTO agent)
        {
            var AgentInfo = _context.Agents.FirstOrDefault(x => x.UserId == id);

            if (AgentInfo == null)
            {
                throw new KeyNotFoundException($"Agent with UserID {id} was not found.");
            }

            AgentInfo.Agent_Name = agent.Agent_Name;
            AgentInfo.ContactInfo = agent.ContactInfo;

            await _context.SaveChangesAsync();
        }


        public async Task<bool> DeleteAgentAsync(int id)
        {
            var agent = await _context.Agents.Include(a => a.AssignedPolicies).FirstOrDefaultAsync(a => a.AgentID == id);
            if (agent == null)
            {
                return false;
            }
            // Find another agent who doesn't have any policies assigned
            var agentWithoutPolicies = await _context.Agents
                                                     .FirstOrDefaultAsync(a => !a.AssignedPolicies.Any() && a.AgentID != id);

            if (agentWithoutPolicies == null)
            {
                throw new Exception("No agent available to reassign policies.");
            }// Reassign policies to the agent without policies
            foreach (var policy in agent.AssignedPolicies)
            {
                policy.AgentID = agentWithoutPolicies.AgentID;
            }
            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return true;    // Save changes
        }

        public async Task<bool> AppointAgentToPolicyAsync(int policyId, int agentId)
        {
            var policy = await _context.Policies.FindAsync(policyId);
            if (policy == null)
            {
                return false;
            }

            policy.AgentID = agentId;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Policy>> GetAssignedPoliciesByAgentIdAsync(string name)
        {
            int id = await GetIdByAgentName(name);

            var agent = await _context.Agents
                .Include(a => a.AssignedPolicies)

                .FirstOrDefaultAsync(a => a.AgentID == id);

            return agent?.AssignedPolicies ?? Enumerable.Empty<Policy>();
        }

        public async Task<int> GetIdByAgentName(string name)
        {
            var agentInfo = await _context.Agents.FirstOrDefaultAsync(x => x.Agent_Name == name);
            if (agentInfo == null)
            {
                throw new Exception("Agent not found ");
            }

            int id = agentInfo.AgentID;

            return id;
        }

        public async Task<Agent> GetAgentById(int id)
        {
            var agent = await _context.Agents.FirstOrDefaultAsync(x => x.UserId == id);

            if (agent == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} was not found.");
            }
            return agent;
        }

        public async Task<bool> IsProfileCompleted(int id)
        {
            var user = await _context.Agents.FirstOrDefaultAsync(c => c.UserId == id);

            if (user == null)
            {
                return false;
            }
            // Check if the user has completed their profile
            return true;
        }

    }
}