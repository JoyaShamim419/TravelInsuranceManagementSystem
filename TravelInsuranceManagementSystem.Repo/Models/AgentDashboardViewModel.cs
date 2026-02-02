namespace TravelInsuranceManagementSystem.Repo.Models

{

    // Container for all Dashboard Data

    public class AgentDashboardViewModel

    {

        public int ActivePolicies { get; set; }

        public int ActiveClaims { get; set; }

        public decimal TodayRevenue { get; set; }

        public int OpenTickets { get; set; }

        public List<DashboardActivity> RecentActivities { get; set; } = new List<DashboardActivity>();

    }

    // Helper class to normalize data for the table

    public class DashboardActivity

    {

        public string Type { get; set; } // "Policy", "Claim", "Ticket"

        public string ReferenceId { get; set; }

        public string CustomerName { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string StatusColor { get; set; } // CSS class for badge

    }

}
