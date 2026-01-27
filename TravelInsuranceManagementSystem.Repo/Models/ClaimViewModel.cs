namespace TravelInsuranceManagementSystem.Application.Models
{
    public class ClaimViewModel
    {
        public int ClaimId { get; set; }
        public int PolicyId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Plan { get; set; }
        public string CoverageType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ClaimDate { get; set; }
        public string Status { get; set; }
        public string AgentName { get; set; }
    }
}