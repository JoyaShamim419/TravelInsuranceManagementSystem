using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; // Added this

namespace TravelInsuranceManagementSystem.Application.Models
{
    public class User : IdentityUser<int> // Necessary change
    {
        // IdentityUser already provides Id, Email, and PasswordHash.
        // Your existing FullName and Role remain exactly as they were.

        [Required(ErrorMessage = "Full Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string FullName { get; set; }

        public string Role { get; set; } = "User";

        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // We use the base Identity 'PasswordHash' for security, 
        // but keeping this property for your existing code compatibility
        [NotMapped]
        public string Password { get; set; }
    }
}