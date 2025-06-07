namespace InsuranceManagementSystem.DTOs
{
    public class PolicyDTO
    {
        public string? Policy_Name { get; set; }

        public decimal? IssuredValue { get; set; }
        public decimal PremiumAmount { get; set; }
        public string? CoverageDetails { get; set; }
        public string? ValidityPeriod { get; set; }
        public Boolean IsActive { get; set; }
        public int AgentID { get; set; }
    }
}
