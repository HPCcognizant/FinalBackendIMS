namespace InsuranceManagementSystem.DTOs
{
    public class ClaimDTO
    {
        public int PolicyID { get; set; }
        public int Customer_ID { get; set; }
        public decimal ClaimAmount { get; set; }
    }
}
