using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Interface
{
    public interface ICustomerPolicyServices
    {
        Task<IEnumerable<CustomerPolicy>> GetAllAsync();
        Task<CustomerPolicy> GetByIdAsync(int customerPolicyId);
        Task<CustomerPolicy> AssignPolicyToCustomerAsync(CustomerPoliciesDTO policiesDTO);
        Task<DateOnly> RenewPolicyAsync(int customerId, int policyId);
        Task DeleteAsync(int customerPolicyId);
        Task<decimal> CalculatePayableAmountAsync(int policyId, string paymentFrequency);
        Task<List<Policy>> GetAllPoliciesByCustomerID(int id);
        Task<List<Customer>> GetAllCustomerByPolicyID(int id);


    }
}
