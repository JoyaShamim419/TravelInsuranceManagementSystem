
//using System.ComponentModel.DataAnnotations;

//namespace TravelInsuranceManagementSystem.Application.Models
//{
//    public class User
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string FullName { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        public string Password { get; set; }

//        // Added to handle your @admin and @agent logic
//        public string Role { get; set; } = "User";
//    }
//}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelInsuranceManagementSystem.Application.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped] // This won't be created in the database
        [Compare("Password", ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = "User";
    }
}