using Microsoft.AspNetCore.Mvc;
using TravelInsuranceManagementSystem.Application.Data;
using TravelInsuranceManagementSystem.Application.Models; // Imports your models (and the conflict)
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims; // Imports Microsoft's security tools (and the conflict)

// 👇 THIS IS THE FIX: We give your Model a specific nickname to resolve the conflict
using InsuranceClaim = TravelInsuranceManagementSystem.Application.Models.Claim;

namespace TravelInsuranceManagementSystem.Application.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClaimsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/UserDashboard/ClaimCreate.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // 👇 UPDATED: We use 'InsuranceClaim' here instead of just 'Claim'
        public async Task<IActionResult> Create(InsuranceClaim claim)
        {
            // 1. Get Logged-in User ID
            var userIdString = User.FindFirst("UserId")?.Value;
            if (userIdString == null) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdString);

            // 2. SECURITY CHECK: Verify Ownership
            // "Find a policy with this ID that ALSO belongs to this User"
            var verifiedPolicy = _context.Policies
                .FirstOrDefault(p => p.PolicyId == claim.PolicyId && p.UserId == userId);

            // If verifiedPolicy is null, it means the policy doesn't exist OR it belongs to someone else
            if (verifiedPolicy == null)
            {
                // ADD ERROR TO PAGE
                ModelState.AddModelError("PolicyId", "Invalid Policy ID. You can only file claims for your own active policies.");

                // RETURN USER TO THE FORM
                return View("~/Views/UserDashboard/ClaimCreate.cshtml", claim);
            }

            // 3. Handle File Upload
            if (claim.DocumentFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + claim.DocumentFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await claim.DocumentFile.CopyToAsync(fileStream);
                }

                claim.DocumentPath = uniqueFileName;
            }

            // 4. Set Defaults
            claim.Status = ClaimStatus.Pending;
            claim.ClaimDate = DateTime.Now;

            // 5. Save to Database
            if (ModelState.IsValid)
            {
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction("Claims", "UserDashboard");
            }

            return View("~/Views/UserDashboard/ClaimCreate.cshtml", claim);
        }
    }
}