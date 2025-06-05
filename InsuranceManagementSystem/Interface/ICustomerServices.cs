using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Interface
{
    public interface ICustomerServices
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(int id);
        Task AddCustomer(CustomerDTO customer, string userid, string email);
        Task UpdateCustomer(CustomerDTO customer, int id);
        Task DeleteCustomer(int id);
        Task<bool> IsProfileCompleted(int id);
        Task<int> FIndCustomerIdByEmail(string email);
    }
}
