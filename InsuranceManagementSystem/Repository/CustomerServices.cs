using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Repository
{
    public class CustomerServices : ICustomerServices
    {
        private readonly DatabaseDbContext _context;
        public CustomerServices(DatabaseDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomer(CustomerDTO customer, string userid, string email)
        {

            var CustInfo = new Customer
            {
                Customer_Name = customer.Customer_Name,
                Customer_Email = email,
                Customer_Phone = customer.Customer_Phone,
                Customer_Address = customer.Customer_Address,
                UserId = Convert.ToInt32(userid),
            };
            await _context.Customers.AddAsync(CustInfo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} was not found.");
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Customer_ID == id);
            
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} was not found.");
            }
            return customer;
        }

        public async Task UpdateCustomer(CustomerDTO customer, int id)
        {
            var CustInfo = _context.Customers.FirstOrDefault(x => x.Customer_ID == id);

            if (CustInfo == null) 
            {
                throw new KeyNotFoundException($"Customer with ID {id} was not found.");
            }
            
            CustInfo.Customer_Name = customer.Customer_Name;
            CustInfo.Customer_Phone = customer.Customer_Phone;
            CustInfo.Customer_Address = customer.Customer_Address;
            
            await _context.SaveChangesAsync();
        }

        public async Task<int> FIndCustomerIdByEmail(string email) 
        {
            var CustInfo = await _context.Customers.FirstOrDefaultAsync(c => c.Customer_Email == email);
            if (CustInfo == null) 
            {
                throw new Exception("Customer Not Found");
            }
            return CustInfo.Customer_ID;
        }

        public async Task<bool> IsProfileCompleted(int id)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == id);

            if (user == null)
            {
                return false;
            }
            // Check if the user has completed their profile
            return true;
        }



    }
}
