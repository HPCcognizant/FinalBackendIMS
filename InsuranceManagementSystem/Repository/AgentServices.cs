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

            var agentInfo = new Agent
            {
                Agent_Name = agent.Agent_Name,
                ContactInfo = agent.ContactInfo,
                UserId = uid,   
            };

            _context.Agents.Add(agentInfo);
            await _context.SaveChangesAsync();

            return agentInfo;
        }

        public async Task<Agent> UpdateAgentAsync(int id, AgentDTO agent)
        {
            var AgentInfo = _context.Agents.FirstOrDefault(x => x.AgentID == id);

            if (AgentInfo == null)
            {
                throw new KeyNotFoundException($"Agent with ID {id} was not found.");
            }

            AgentInfo.Agent_Name = agent.Agent_Name;
            AgentInfo.ContactInfo = agent.ContactInfo;

            await _context.SaveChangesAsync();
            return AgentInfo;
        }

        public async Task<bool> DeleteAgentAsync(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null)
            {
                return false;
            }

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return true;
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

        Task IAgentServices.UpdateAgentAsync(int id, object agent_Name)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Policy>> GetAssignedPoliciesByAgentIdAsync(int agentId)
        {
            var agent = await _context.Agents
                .Include(a => a.AssignedPolicies)
                .FirstOrDefaultAsync(a => a.AgentID == agentId);

            return agent?.AssignedPolicies ?? Enumerable.Empty<Policy>();
        }


    }
}