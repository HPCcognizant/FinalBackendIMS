namespace InsuranceManagementSystem.DTOs
{
    public class NotificationDTO
    {
        public int CustomerID { get; set; }
        public string? Message { get; set; }
        public DateOnly Datasent { get; set; }
    }
}
