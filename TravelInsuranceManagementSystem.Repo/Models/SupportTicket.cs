using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelInsuranceManagementSystem.Repo.Models; // Add this if needed

namespace TravelInsuranceManagementSystem.Repo.Models
{
    public class SupportTicket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string IssueDescription { get; set; }

        [Required]
        public string TicketStatus { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ResolvedDate { get; set; }

        // Navigation property
        public virtual TicketDetail ExtensionData { get; set; }
    }
}