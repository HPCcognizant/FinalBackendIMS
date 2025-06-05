using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Repository
{
    public class PolicyServices : IPolicyServices
    {
        private readonly DatabaseDbContext _context;
        public PolicyServices(DatabaseDbContext context)
        {
            _context = context;
        }

        public async Task<Policy> AddAsync(PolicyDTO policy)
        {
            var newPolicy = new Policy
            {
                Policy_Name = policy.Policy_Name,
                PremiumAmount = policy.PremiumAmount,
                CoverageDetails = policy.CoverageDetails,
                ValidityPeriod = policy.ValidityPeriod,
                AgentID = policy.AgentID
            };
            await _context.Policies.AddAsync(newPolicy);
            await _context.SaveChangesAsync();
            return newPolicy;
        }

        public async Task DeleteAsync(int policyId)
        {
            var policy = await _context.Policies.FindAsync(policyId);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Policy>> GetAllAsync()
        {
            return await _context.Policies.ToListAsync() ?? throw new NotImplementedException();
        }

        public async Task<List<Policy>> GetAllActivePoliciesAsync()
        {
            List<Policy> policy = await _context.Policies.Where(policy => policy.IsActive).ToListAsync();

            if (policy == null) {
                throw new ArgumentNullException("No Active Policies available");
            }

            return policy;
        }

        public async Task UpdatePolicyValidity()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var experiedPolicies = await _context.Policies.Where( p => p.ValidityPeriod < today && p.IsActive).ToListAsync();

            foreach (var policy in  experiedPolicies) 
            {
                policy.IsActive = false;
                _context.SaveChanges();
            }
        }

        public async Task<Policy> GetByIdAsync(int policyId)
        {
            var policy = await _context.Policies.Include(p=>p.Agent).FirstOrDefaultAsync(x => x.PolicyID == policyId);
            return policy;
        }

        public async Task<Policy> UpdateAsync(int policyId, PolicyDTO policy)
        {
            // Fix for CS0029 and CS1662: Corrected the lambda expression to use '==' for comparison
            var existingPolicy = await _context.Policies.FirstOrDefaultAsync(x => x.PolicyID == policyId);

            if (existingPolicy != null)
            {
                // Update properties of the existing policy
                existingPolicy.Policy_Name = policy.Policy_Name;
                existingPolicy.ValidityPeriod = policy.ValidityPeriod;
                existingPolicy.PremiumAmount = policy.PremiumAmount; // Fixed incorrect property 'P'
                existingPolicy.CoverageDetails = policy.CoverageDetails;
                existingPolicy.IsActive = policy.IsActive;
                existingPolicy.AgentID = policy.AgentID;

                // Save changes to the database
                await _context.SaveChangesAsync();
                return existingPolicy;
            }
            return null;
        }




    }
}