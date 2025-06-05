namespace InsuranceManagementSystem.DTOs
{
    public class NotificationDTO
    {
        public string Email { get; set; }
        public string? Message { get; set; }
        public DateOnly Datasent { get; set; }
    }
}
