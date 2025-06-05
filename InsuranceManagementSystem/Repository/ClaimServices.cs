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

        // Constructor for ClaimRepo
        public ClaimServices(DatabaseDbContext context)
        {
            _context = context;
        }

        public async Task<(bool IsSuccess, string Message)> AddClaimToDb(ClaimDTO claim)
        {
            var existingClaim = await _context.Claims.FirstOrDefaultAsync(c => c.Customer_ID == claim.Customer_ID && c.PolicyID == claim.PolicyID && c.Status != "Rejected");

            if (existingClaim != null)
            {
                return (false, $"A Claim has already been filed and is currently under '{existingClaim.Status}'.");
            }

            // Map ClaimDTO to Claim
            var claimEntity = new Claim
            {
                PolicyID = claim.PolicyID,
                Customer_ID = claim.Customer_ID,
                ClaimAmount = claim.ClaimAmount
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

        public async Task<Claim> UpdateClaimStatus(int id, string claim)
        {
            var ClaimInfo = _context.Claims.FirstOrDefault(x => x.ClaimID == id);

            if (ClaimInfo == null)
            {
                throw new KeyNotFoundException($"Claim with id {id} was not found");
            }

            ClaimInfo.Status = claim;

            await _context.SaveChangesAsync();
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
     
    }
}