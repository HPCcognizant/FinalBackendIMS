using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Interface
{
    public interface IClaimServices
    {
        Task<(bool IsSuccess, string Message)> AddClaimToDb(ClaimDTO claim);
        Task<Claim> GetClaimByIdFromDb(int id);
        Task<List<Claim>> GetAllClaimsFromDb();

        Task<List<Claim>> GetClaimsByCustomerId(int customerId);
        Task<Claim> UpdateClaimStatus(int id, string claim, string reason);
        Task DeleteClaimFromDb(int id);

    }
}