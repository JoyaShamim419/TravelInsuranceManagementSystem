using System;
using System.Collections.Generic; // Added for List<>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelInsuranceManagementSystem.Application.Models
{
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        // Made nullable so it doesn't fail if User session is empty
        public int? UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [StringLength(100)]
        public string DestinationCountry { get; set; } = string.Empty; // Initialize to empty string

        [Required]
        public DateTime TravelStartDate { get; set; }

        [Required]
        public DateTime TravelEndDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CoverageAmount { get; set; }

        [StringLength(100)]
        public string CoverageType { get; set; } = string.Empty; // Initialize to empty string

        [Required]
        public PolicyStatus PolicyStatus { get; set; }

        // Changed to public set to allow EF to track the collection properly
        public List<PolicyMember> Members { get; set; } = new List<PolicyMember>();
    }

    public enum PolicyStatus
    {
        ACTIVE,
        EXPIRED,
        CANCELLED
    }

    public class PolicyMember
    {
        [Key]
        public int MemberId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Relation { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Mobile { get; set; }

        // Foreign Key to Policy
        public int PolicyId { get; set; }

        // Use 'null!' to ignore warning, as EF Core handles the relationship
        public  Policy Policy { get; set; } ;
    }
}