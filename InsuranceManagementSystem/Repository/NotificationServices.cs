using InsuranceManagementSystem.Data;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Repository
{
    public class NotificationServices : INotificationServices
    {
        private readonly DatabaseDbContext _context;
        public NotificationServices(DatabaseDbContext context) 
        {
            _context = context;
        }

        public async Task<int> GetCustomerIdByEmail(string email)
        {
            if( email == null ) { throw new ArgumentNullException("email cannot be null"); }

            var CustInfo = await _context.Customers.FirstOrDefaultAsync(c => c.Customer_Email == email);

            if (CustInfo == null) { throw new ArgumentNullException(" Customer Not Found "); }

            return CustInfo.Customer_ID;
        }

        public async Task<List<Notification>> GetNotificationById(int id)
        {
            List<Notification> notifications = await _context.Notifications.Where(c=>c.Customer_ID == id).OrderByDescending(n =>n.NotificationID).Take(5).ToListAsync();

            if (notifications == null) { throw new ArgumentNullException("No Notifications"); }

            return notifications;
        }
         
        public async Task<Notification> SentNotificationToCustomer(NotificationDTO notificationDTO)
        {
            if (notificationDTO == null) { throw new ArgumentNullException("Notification cannot be null"); }

            int CustId = await GetCustomerIdByEmail(notificationDTO.Email);

            var notification = new Notification 
            { 
                Customer_ID = CustId,
                Message = notificationDTO.Message,  
                Datasent = notificationDTO.Datasent,    
            };

            await _context.Notifications.AddAsync(notification);
            await  _context.SaveChangesAsync();

            return notification;
        }

        public async Task<Notification> SentNotificationAfterApproval(int id, string reason)
        {

            var notification = new Notification
            {
                Customer_ID = id,
                Message = reason,
                Datasent = DateOnly.FromDateTime(DateTime.Now)
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<Notification> SentNotificationAfterRejected(int id, string reason)
        {

            var notification = new Notification
            {
                Customer_ID = id,
                Message = reason,
                Datasent = DateOnly.FromDateTime(DateTime.Now)
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<Notification> SentNotificationAfterUnderReview(int id)
        {

            var notification = new Notification
            {
                Customer_ID = id,
                Message = "Your Claim is under Review.",
                Datasent = DateOnly.FromDateTime(DateTime.Now)
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            return notification;
        }
    }
}
