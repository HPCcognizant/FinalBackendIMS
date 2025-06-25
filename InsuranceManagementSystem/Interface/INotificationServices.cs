using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Models;

namespace InsuranceManagementSystem.Interface
{
    public interface INotificationServices
    {
        Task<Notification> SentNotificationToCustomer(NotificationDTO notificationDTO);
        Task<List<Notification>> GetNotificationById(int id);
        Task<Notification> SentNotificationAfterApproval(int id, string reason);
        Task<Notification> SentNotificationAfterRejected(int id, string reason);
        Task<Notification> SentNotificationAfterUnderReview(int id);
        Task<int> GetCustomerIdByEmail(string email); 
    }
}
