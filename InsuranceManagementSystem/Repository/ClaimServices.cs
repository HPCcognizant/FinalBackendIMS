using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Repository
{
    public class ClaimServices : IClaimServices
    {
        private readonly DatabaseDbContext _context;
        private readonly INotificationServices _notificationServices;

        // Constructor for ClaimRepo
        public ClaimServices(DatabaseDbContext context, INotificationServices notificationServices)
        {
            _context = context;
            _notificationServices = notificationServices;
        }

        public async Task<(bool IsSuccess, string Message)> AddClaimToDb(ClaimDTO claim)
        {
            // Get all approved claims for this customer and policy
            var approvedClaims = await _context.Claims
                .Where(c => c.Customer_ID == claim.Customer_ID && c.PolicyID == claim.PolicyID && c.Status == "Approved")
                .ToListAsync();

            // Calculate total claimed so far
            decimal totalClaimed = approvedClaims.Sum(c => c.ClaimAmount);

            // Get policy
            var policy = await _context.Policies.FindAsync(claim.PolicyID);
            if (policy == null)
                return (false, $"Policy with id {claim.PolicyID} was not found.");

            decimal insuredValue = policy.IssuredValue ?? 0;
            decimal remainingAmount = insuredValue - totalClaimed;

            // Check for invalid claim amount
            if (claim.ClaimAmount <= 0)
                return (false, "Invalid claim amount. Amount must be greater than zero.");

            if (claim.ClaimAmount > remainingAmount)
                return (false, $"Invalid claim amount. You can only claim up to {remainingAmount}.");

            // Check for any pending (not rejected/approved) claim
            var pendingClaim = await _context.Claims.FirstOrDefaultAsync(c =>
                c.Customer_ID == claim.Customer_ID &&
                c.PolicyID == claim.PolicyID &&
                c.Status != "Rejected" &&
                c.Status != "Approved");

            if (pendingClaim != null)
            {
                return (false, $"A Claim has already been filed and is currently under '{pendingClaim.Status}'.");
            }

            // Map ClaimDTO to Claim
            var claimEntity = new Claim
            {
                PolicyID = claim.PolicyID,
                Customer_ID = claim.Customer_ID,
                ClaimAmount = claim.ClaimAmount,
                ClaimReason = claim.ClaimReason,
                RemainigCAmount = remainingAmount - claim.ClaimAmount
            };

            await _context.Claims.AddAsync(claimEntity);
            await _context.SaveChangesAsync();
            return (true, "Claim Filed Successfully.");
        }


        public async Task<Claim> GetClaimByIdFromDb(int id)
        {
            var ClaimInfo = await _context.Claims.FindAsync(id);

            if (ClaimInfo == null)
            {
                throw new KeyNotFoundException($"Claim with id {id} was not found");
            }
            return ClaimInfo;
        }

        public async Task<List<Claim>> GetAllClaimsFromDb()
        {
            return await _context.Claims.Include(c => c.Customer).ThenInclude(c => c.CustomerPolicies).ThenInclude(c => c.Policy).ToListAsync();
        }

        public async Task<Claim> UpdateClaimStatus(int id, string claim, string reason)
        {
            var ClaimInfo = _context.Claims.FirstOrDefault(x => x.ClaimID == id);

            if (ClaimInfo == null)
            {
                throw new KeyNotFoundException($"Claim with id {id} was not found");
            }

            ClaimInfo.Status = claim;
            ClaimInfo.AdminReason = reason;

            await _context.SaveChangesAsync();

            if(ClaimInfo.Status == "Approved")
            {
               await  _notificationServices.SentNotificationAfterApproval(ClaimInfo.Customer_ID, ClaimInfo.AdminReason);
            }
            if (ClaimInfo.Status == "Rejected") { 
               await _notificationServices.SentNotificationAfterRejected(ClaimInfo.Customer_ID, ClaimInfo.AdminReason);
            }
            if (ClaimInfo.Status == "Under Review")
            {
                await _notificationServices.SentNotificationAfterUnderReview(ClaimInfo.Customer_ID);
            }

            return ClaimInfo;
        }

        public async Task DeleteClaimFromDb(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                throw new KeyNotFoundException($"Claim with id {id} was not found");
            }
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Claim>> GetClaimsByCustomerId(int customerId)
        {
            return await _context.Claims
                .Where(c => c.Customer_ID == customerId)
                .Include(c => c.Policy)
                .ToListAsync();
        }

        }
}