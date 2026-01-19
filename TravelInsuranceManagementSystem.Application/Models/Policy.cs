using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelInsuranceManagementSystem.Application.Models
{
    public class Policy
    {
        [Key] // Defines policyId as the Primary Key
        public int PolicyId { get; set; }
        public int UserId { get; set; }
        public User User
        {
            get; set;
        }

            [Required]
        [StringLength(100)]
        public string DestinationCountry { get; set; }

        [Required]
        public DateTime TravelStartDate { get; set; }

        [Required]
        public DateTime TravelEndDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")] // Matches your decimal(10,2) requirement
        public decimal CoverageAmount { get; set; }

        [StringLength(100)]
        public string CoverageType { get; set; }

        [Required]
        public PolicyStatus PolicyStatus { get; set; }
        public List<PolicyMember> Members { get; internal set; }
    }

    // Enum to handle the specific status options
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
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relation { get; set; }
        public DateTime DOB { get; set; }
        public string Mobile { get; set; }
        public List<PolicyMember> Members { get; set; } = new List<PolicyMember>();


        // Foreign Key to Policy
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }
    }
}