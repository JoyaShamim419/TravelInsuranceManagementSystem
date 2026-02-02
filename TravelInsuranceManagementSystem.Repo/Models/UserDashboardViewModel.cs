namespace TravelInsuranceManagementSystem.Repo.Models

{

    public class UserDashboardViewModel

    {

        public int ActivePolicies { get; set; }

        public int OpenClaims { get; set; }

        public int Tickets { get; set; }

        public int UpcomingTrips { get; set; }

        public List<UserDashboardActivity> RecentActivities { get; set; } = new List<UserDashboardActivity>();

    }

    public class UserDashboardActivity

    {

        public string Type { get; set; } // "Policy", "Claim", "Ticket"

        public string ReferenceId { get; set; }

        public string Description { get; set; } // e.g., "Trip to Paris" or "Medical Expense"

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string StatusColor { get; set; } // CSS class for badge

    }

}
