using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Repository
{
    public class CustomerPolicyServices : ICustomerPolicyServices
    {
        private readonly DatabaseDbContext _context;

        public CustomerPolicyServices(DatabaseDbContext context)
        {
            _context = context;
        }

        // Assign a Policy to a Customer
        public async Task<CustomerPolicy> AssignPolicyToCustomerAsync(CustomerPoliciesDTO policiesDTO)
        {

            var customerExists = await _context.Customers.AnyAsync(c => c.Customer_ID == policiesDTO.Customer_ID);
            var policyExists = await _context.Policies.AnyAsync(p => p.PolicyID == policiesDTO.PolicyID);

            if (!customerExists || !policyExists)
                throw new ArgumentException("Invalid CustomerID or PolicyID.");

            var customerPolicy = new CustomerPolicy
            {
                Customer_ID = policiesDTO.Customer_ID,
                PolicyID = policiesDTO.PolicyID,
            };

            _context.CustomerPolicies.Add(customerPolicy);
            await _context.SaveChangesAsync();
            return customerPolicy;
        }

        // Delete an Assigned Policy
        public async Task DeleteAsync(int customerPolicyId)
        {
            var customerPolicy = await _context.CustomerPolicies.FindAsync(customerPolicyId);
            if (customerPolicy != null)
            {
                _context.CustomerPolicies.Remove(customerPolicy);
                await _context.SaveChangesAsync();
            }
        }

        // Retrieve All Assigned Policies
        public async Task<IEnumerable<CustomerPolicy>> GetAllAsync()
        {
            return await _context.CustomerPolicies
                                 .Include(cp => cp.Customer)
                                 .Include(cp => cp.Policy)
                                 .ToListAsync();
        }

        // Retrieve a Policy Assignment by ID
        public async Task<CustomerPolicy> GetByIdAsync(int customerPolicyId)
        {
            return await _context.CustomerPolicies
                                 .Include(cp => cp.Customer)
                                 .Include(cp => cp.Policy)
                                 .FirstOrDefaultAsync(cp => cp.CustomerPolicy_ID == customerPolicyId);
        }

        // Update Assigned Policy
        public async Task<CustomerPolicy> UpdateAssignedPolicyAsync(int customerPolicyId, CustomerPoliciesDTO policiesDTO)
        {
            var customerPolicy = await _context.CustomerPolicies.FindAsync(customerPolicyId);
            if (customerPolicy == null)
                throw new ArgumentException("Customer Policy record not found.");

            var customerExists = await _context.Customers.AnyAsync(c => c.Customer_ID == policiesDTO.Customer_ID);
            var policyExists = await _context.Policies.AnyAsync(p => p.PolicyID == policiesDTO.PolicyID);

            if (!customerExists || !policyExists)
                throw new ArgumentException("Invalid CustomerID or PolicyID.");

            customerPolicy.Customer_ID = policiesDTO.Customer_ID;
            customerPolicy.PolicyID = policiesDTO.PolicyID;

            await _context.SaveChangesAsync();
            return customerPolicy;
        }

        public async Task<List<Policy>> GetAllPoliciesByCustomerID(int id) 
        {
            var policyIds = await _context.CustomerPolicies.Where(c => c.Customer_ID == id).Select(cp => cp.PolicyID).ToListAsync();

             List<Policy> policies = await _context.Policies.Where(p => policyIds.Contains(p.PolicyID)).ToListAsync();

            return policies;
        }

        public async Task<List<Customer>> GetAllCustomerByPolicyID(int id)
        {
            var CustIds = await _context.CustomerPolicies.Where(c => c.PolicyID == id).Select(cp => cp.Customer_ID).ToListAsync();

            List<Customer> customers = await _context.Customers.Where(p => CustIds.Contains(p.Customer_ID)).ToListAsync();

            return customers;
        }
    }
}