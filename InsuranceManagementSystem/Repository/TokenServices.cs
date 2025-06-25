using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.IdentityModel.Tokens;

namespace InsuranceManagementSystem.Repository
{
    public class TokenServices : ITokenGenerate
    {
        private readonly SymmetricSecurityKey _key;

        public TokenServices(IConfiguration configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]!));
        }

        public string GenerateToken(User user)
        {
            string token = string.Empty;
            var claims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim("Username", user.Username!),
                        new System.Security.Claims.Claim("Email", user.Email!),
                        new System.Security.Claims.Claim(ClaimTypes.Role, user.role),
                        new System.Security.Claims.Claim("UserId",user.Id.ToString())
                    };

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var myToken = tokenHandler.CreateToken(tokenDescription);
            token = tokenHandler.WriteToken(myToken);
            return token;
        }

    }
}