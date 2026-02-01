using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelInsuranceManagementSystem.Repo.Models
{
    public class FamilyInsuranceDto
    {
        [Required]
        public PolicyDto PolicyDetails { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one family member is required.")]
        public List<MemberDto> Members { get; set; } = new List<MemberDto>();

        [Required]
        public NomineeDto Nominee { get; set; }

        [Required]
        public DeclarationDto Declarations { get; set; }
    }

    public class PolicyDto
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime TripStart { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TripEnd { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string PlanType { get; set; }

        [Required]
        [EmailAddress]
        public string PrimaryEmail { get; set; }

        [Required]
        [RegularExpression(@"^\d{10,12}$", ErrorMessage = "Mobile must be 10-12 digits.")]
        public string PrimaryMobile { get; set; }
    }

    public class MemberDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Relation { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        public string Mobile { get; set; }
    }

    public class NomineeDto
    {
        [Required]
        public string NomineeName { get; set; }
        [Required]
        public string NomineeRelation { get; set; }
        [Required]
        public string NomineeMobile { get; set; }
    }

    public class DeclarationDto
    {
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must be a resident.")]
        public bool IsResident { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must declare PEP status.")]
        public bool IsNotPEP { get; set; }
    }
}