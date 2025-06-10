using System.Collections.Specialized;
using System.Net.Mail;
using InsuranceManagementSystem.DTOs;
using InsuranceManagementSystem.Interface;
using InsuranceManagementSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationServices _services;
        public NotificationsController(INotificationServices services) 
        {
            _services = services;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Agent")]
        public async Task<IActionResult> NotifyCustomer(NotificationDTO notification) 
        {
            if (notification == null) 
            {
                return NotFound("notification is empty");
            }

            var result = await _services.SentNotificationToCustomer(notification);

            if (result == null) {
                throw new Exception("Erorr in notification service");
            }

            return Ok(result);  
        }


        [HttpGet]
        public async Task<IActionResult> GetTheCustomerSpecificNotification(int id ) 
        {
            var notifications = await _services.GetNotificationById(id);
           
            if (notifications == null) {
                return Ok("No new Notification");
            }

            return Ok(notifications);
        }
    }
}
