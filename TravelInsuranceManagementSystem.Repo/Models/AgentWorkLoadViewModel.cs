namespace TravelInsuranceManagementSystem.Models.ViewModels

{

    public class AgentWorkloadViewModel

    {

        public int Id { get; set; } // User ID (int based on your User model)

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        // Workload Stats

        public int PoliciesHandled { get; set; }

        public int ClaimsHandled { get; set; }

        public int TicketsHandled { get; set; }

        public bool IsActive { get; set; } = true; // Default status

    }

}
