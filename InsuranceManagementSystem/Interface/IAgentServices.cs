using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Services
{
    public interface IAgentServices
    {
        Task<IEnumerable<Agent>> GetAllAgentsAsync();
        Task<Agent> GetAgentByIdAsync(int id);
        Task<Agent> AddAgentAsync(AgentDTO agent, string userid);
        Task UpdateAgentAsync(int id, AgentDTO agent);
        Task<bool> DeleteAgentAsync(int id);
        Task<bool> IsProfileCompleted(int id);
        Task<bool> AppointAgentToPolicyAsync(int policyId, int agentId);
        Task<IEnumerable<Policy>> GetAssignedPoliciesByAgentIdAsync(string name);
    }
}
