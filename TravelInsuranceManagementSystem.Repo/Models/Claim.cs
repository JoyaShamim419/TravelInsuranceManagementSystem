using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace TravelInsuranceManagementSystem.Repo.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        [ForeignKey("Policy")]
        public int PolicyId { get; set; }
        public virtual Policy? Policy { get; set; }

        [Required]
        [StringLength(50)]
        public string IncidentType { get; set; }

        [DataType(DataType.Date)]
        public DateTime IncidentDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal ClaimAmount { get; set; }

        [DataType(DataType.Date)]
        public DateTime ClaimDate { get; set; } = DateTime.Now;

        public string Description { get; set; }

        // We use the Enum here
        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public int AgentId { get; set; }

        [StringLength(100)]
        public string? Remarks { get; set; }

        public string? DocumentPath { get; set; }

        [NotMapped]
        public IFormFile? DocumentFile { get; set; }

    }

    // Enum defined in the same file for simplicity
    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }
}