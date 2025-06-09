using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        // Assign a Policy to a Customer
        public async Task<CustomerPolicy> AssignPolicyToCustomerAsync(CustomerPoliciesDTO policiesDTO)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var customerExists = await _context.Customers.AnyAsync(c => c.Customer_ID == policiesDTO.Customer_ID);
            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyID == policiesDTO.PolicyID);

            if (!customerExists || policy == null)
                throw new ArgumentException("Invalid CustomerID or PolicyID");

            // Check for active policy
            var existingPolicy = await _context.CustomerPolicies
                .FirstOrDefaultAsync(cp =>
                    cp.Customer_ID == policiesDTO.Customer_ID &&
                    cp.PolicyID == policiesDTO.PolicyID &&
                    cp.EndDate > today
                );

            if (existingPolicy != null)
                throw new ArgumentException("Customer already has an active policy of this type.");

            // Calculate EndDate based on StartDate and Policy.ValidityPeriod
            DateOnly endDate = policiesDTO.StartDate;
            try
            {
                if (!string.IsNullOrEmpty(policy.ValidityPeriod))
                {
                    endDate = DateOnly.FromDateTime(
                        GetNextRenewDate(policiesDTO.StartDate.ToDateTime(TimeOnly.MinValue), policy.ValidityPeriod)
                    );
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"For validity period '{policy.ValidityPeriod}', the selected payment frequency is not applicable.");
            }

            // Calculate first RenewDate based on StartDate and PaymentFrequency
            DateOnly renewDate = policiesDTO.StartDate;
            try
            {
                if (!string.IsNullOrEmpty(policiesDTO.PaymentFrequency))
                {
                    renewDate = DateOnly.FromDateTime(
                        GetNextRenewDate(policiesDTO.StartDate.ToDateTime(TimeOnly.MinValue), policiesDTO.PaymentFrequency)
                    );
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"For validity period '{policy.ValidityPeriod}', the selected payment frequency '{policiesDTO.PaymentFrequency}' is not applicable.");
            }

            var customerPolicy = new CustomerPolicy
            {
                Customer_ID = policiesDTO.Customer_ID,
                PolicyID = policiesDTO.PolicyID,
                StartDate = policiesDTO.StartDate,
                EndDate = endDate,
                RenewDate = renewDate,
                PayableAmount = policiesDTO.PayableAmount,
                PaymentFrequency = policiesDTO.PaymentFrequency
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

        private DateTime GetNextRenewDate(DateTime fromDate, string frequency)
        {
            return frequency.ToLower() switch
            {
                "monthly" => fromDate.AddMonths(1),
                "quarterly" => fromDate.AddMonths(3),
                "half yearly" => fromDate.AddMonths(6),
                "yearly" => fromDate.AddYears(1),
                _ => throw new ArgumentException("Invalid payment frequency")
            };
        }

        // Fix for CS8604: Ensure 'PaymentFrequency' is not null before passing it to 'GetNextRenewDate'.
        public async Task<DateOnly> RenewPolicyAsync(int customerId, int policyId)
        {
            var customerPolicy = await _context.CustomerPolicies
                .FirstOrDefaultAsync(cp => cp.Customer_ID == customerId && cp.PolicyID == policyId);

            if (customerPolicy == null)
                throw new ArgumentException("Policy record not found for this customer.");

            if (string.IsNullOrEmpty(customerPolicy.PaymentFrequency))
                throw new ArgumentException("Payment frequency is not set for this policy.");

            var today = DateOnly.FromDateTime(DateTime.Now);

            // Define how many days before renew date renewal is allowed
            int renewWindowDays = 7; // e.g., allow renewal only within 7 days before renew date

            if (customerPolicy.RenewDate > today.AddDays(renewWindowDays))
                throw new ArgumentException($"Policy is already renewed. Next renew date: {customerPolicy.RenewDate:yyyy-MM-dd}");

            // If RenewDate is in the past or within the window, allow renewal
            var baseDate = customerPolicy.RenewDate > today
                ? customerPolicy.RenewDate
                : today;

            customerPolicy.RenewDate = DateOnly.FromDateTime(
                GetNextRenewDate(baseDate.ToDateTime(TimeOnly.MinValue), customerPolicy.PaymentFrequency)
            );

            _context.CustomerPolicies.Update(customerPolicy);
            await _context.SaveChangesAsync();
            return customerPolicy.RenewDate;
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

        public async Task<decimal> CalculatePayableAmountAsync(int policyId, string paymentFrequency)
        {
            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyID == policyId);
            if (policy == null)
                throw new ArgumentException("Policy not found");

            // Validate payment frequency against policy validity period
            if (!IsPaymentFrequencyAllowed(policy.ValidityPeriod, paymentFrequency))
                throw new ArgumentException($"Payment frequency '{paymentFrequency}' is not allowed for policy validity period '{policy.ValidityPeriod}'.");

            decimal baseAmount = policy.PremiumAmount;
            decimal payableAmount;

            // If payment frequency matches validity period, charge full premium amount
            if (!string.IsNullOrEmpty(policy.ValidityPeriod) &&
                paymentFrequency.Equals(policy.ValidityPeriod, StringComparison.OrdinalIgnoreCase))
            {
                payableAmount = baseAmount;
            }
            else
            {
                switch (paymentFrequency.ToLower())
                {
                    case "monthly":
                        payableAmount = (baseAmount / 12) * 1.05m;  // 5% interest
                        break;
                    case "quarterly":
                        payableAmount = (baseAmount / 4) * 1.03m;   // 3% interest
                        break;
                    case "half yearly":
                        payableAmount = (baseAmount / 2) * 1.02m;   // 2% interest
                        break;
                    case "yearly":
                        payableAmount = baseAmount;                // no interest
                        break;
                    default:
                        throw new ArgumentException("Invalid payment frequency");
                }
            }

            return Math.Round(payableAmount, 2);
        }


        // Helper method to validate allowed payment frequencies
        private bool IsPaymentFrequencyAllowed(string? validityPeriod, string paymentFrequency)
        {
            if (string.IsNullOrEmpty(validityPeriod))
                return true; // or false, depending on your business rule

            switch (validityPeriod.ToLower())
            {
                case "halfyearly":
                    // Only allow monthly and quarterly
                    return paymentFrequency.Equals("monthly", StringComparison.OrdinalIgnoreCase)
                        || paymentFrequency.Equals("quarterly", StringComparison.OrdinalIgnoreCase);
                case "yearly":
                    // Allow all frequencies
                    return true;
                // Add more cases as needed
                default:
                    return true;
            }
        }

    }
}