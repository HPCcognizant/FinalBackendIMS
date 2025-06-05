using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceManagementSystem.Interface
{
    public interface IUserServices
    {
        Task<bool> RegisterOfUser(UserDTO user);
        //Task<string> LoginOfUser(string username, string password);

        Task<bool> RegisterAgent(UserDTO user);
        Task<ICollection<User>> GetAllUser();
    }
}
