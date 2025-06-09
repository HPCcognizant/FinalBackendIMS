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
            // Check for existing phone number
            if (await _context.Customers.AnyAsync(c => c.Customer_Phone == customer.Customer_Phone))
                throw new ArgumentException("Customer phone number already exists. Please use a different phone number.");

            var CustInfo = new Customer
            {
                Customer_Name = customer.Customer_Name,
                Customer_Email = email,
                Customer_Phone = customer.Customer_Phone,
                Customer_Address = customer.Customer_Address,
                UserId = Convert.ToInt32(userid),
            };

            try
            {
                await _context.Customers.AddAsync(CustInfo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Fallback in case of a race condition or other unique constraint violation
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                {
                    throw new ArgumentException("Customer phone number already exists. Please use a different phone number.");
                }
                throw;
            }
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
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.UserId == id);
            
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} was not found.");
            }
            return customer;
        }

        public async Task UpdateCustomer(CustomerDTO customer,int id)
        {
            var CustInfo = _context.Customers.FirstOrDefault(x => x.UserId == id);

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
