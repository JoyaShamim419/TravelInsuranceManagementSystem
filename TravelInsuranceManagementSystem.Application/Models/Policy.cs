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

        public int UserId { get; set; }

        // 👇 FIX 1: Add 'required' or '?' to silence the warning.
        // Since UserId is 'int' (not nullable), the User object MUST exist in the DB.
        // We use 'null!' to tell C# "Trust me, EF Core will fill this, it won't be null".
        public virtual User User { get; set; } = null!;

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

        // 👇 FIX 2: Initialize the list to prevent null reference errors
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
        public string Mobile { get; set; } = string.Empty;

        // 👇 FIX 3: Remove this nested List<PolicyMember>. 
        // A member doesn't have a list of members inside it. That's a circular reference loop.
        // public List<PolicyMember> Members { get; set; } = new List<PolicyMember>(); 

        // Foreign Key to Policy
        public int PolicyId { get; set; }

        // Use 'null!' to ignore warning, as EF Core handles the relationship
        public virtual Policy Policy { get; set; } = null!;
    }
}