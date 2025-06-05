using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Interface
{
    public interface ICustomerPolicyServices
    {
        Task<IEnumerable<CustomerPolicy>> GetAllAsync();
        Task<CustomerPolicy> GetByIdAsync(int customerPolicyId);
        Task<CustomerPolicy> AssignPolicyToCustomerAsync(CustomerPoliciesDTO Policies);
        Task<CustomerPolicy> UpdateAssignedPolicyAsync(int customerPolicyId, CustomerPoliciesDTO Policies);
        Task DeleteAsync(int customerPolicyId);

        Task<List<Policy>> GetAllPoliciesByCustomerID(int id);
        Task<List<Customer>> GetAllCustomerByPolicyID(int id);


    }
}
