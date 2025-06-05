using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;

namespace InsuranceManagementSystem.Interface
{
    public interface ITokenGenerate
    {
        public string GenerateToken(User user);

    }
}