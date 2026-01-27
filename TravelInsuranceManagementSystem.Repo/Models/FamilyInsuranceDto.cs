using System;
using System.Collections.Generic;

namespace TravelInsuranceManagementSystem.Application.Models
{
    public class FamilyInsuranceDto
    {
        public PolicyDto PolicyDetails { get; set; }
        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
        public NomineeDto Nominee { get; set; }
        public DeclarationDto Declarations { get; set; }
    }

    public class PolicyDto
    {
        public DateTime TripStart { get; set; }
        public DateTime TripEnd { get; set; }
        public string Destination { get; set; }
        public string PlanType { get; set; }
        public string PrimaryEmail { get; set; }
        public string PrimaryMobile { get; set; }
    }

    public class MemberDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relation { get; set; }
        public DateTime DOB { get; set; }
        public string Mobile { get; set; }
    }

    public class NomineeDto
    {
        public string NomineeName { get; set; }
        public string NomineeRelation { get; set; }
        public string NomineeMobile { get; set; }
    }

    public class DeclarationDto
    {
        public bool IsResident { get; set; }
        public bool IsNotPEP { get; set; }
    }
}