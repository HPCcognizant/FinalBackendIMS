using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Interface
{
    public interface IPolicyServices
    {
        Task<IEnumerable<Policy>> GetAllAsync();
        Task<Policy> GetByIdAsync(int policyId);
        Task<Policy> AddAsync(PolicyDTO policy);
        Task<Policy> UpdateAsync(int policyId, PolicyDTO policy);

        Task<List<Policy>> GetAllActivePoliciesAsync();
        Task DeleteAsync(int policyId);
    }
}
