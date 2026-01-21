using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    public class InsuranceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsuranceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult FamilyInsurance() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFamily([FromBody] FamilyInsuranceDto data)
        {
            if (data == null || data.PolicyDetails == null)
                return BadRequest("No data received.");

            try
            {
                var newPolicy = new Policy
                {
                    DestinationCountry = data.PolicyDetails.Destination,
                    TravelStartDate = data.PolicyDetails.TripStart,
                    TravelEndDate = data.PolicyDetails.TripEnd,
                    CoverageType = data.PolicyDetails.PlanType,
                    PolicyStatus = PolicyStatus.ACTIVE,

                    // KEPT: Your original coverage logic
                    CoverageAmount = data.PolicyDetails.PlanType == "Premium" ? 50000 : 10000,

                    // KEPT: Your member mapping logic
                    Members = data.Members.Select(m => new PolicyMember
                    {
                        Title = m.Title,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Relation = m.Relation,
                        DOB = m.DOB,
                        Mobile = m.Mobile
                    }).ToList()
                };

                _context.Policies.Add(newPolicy);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Policy generated successfully!", id = newPolicy.PolicyId });
            }
            catch (Exception ex)
            {
                // IMPROVED: Now tells you exactly which database field failed
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, "Internal server error: " + innerMsg);
            }
        }

        [HttpGet]
        public IActionResult Success(int id)
        {
            ViewBag.PolicyDisplayId = "P-" + id.ToString("D5");
            return View();
        }
    }
}