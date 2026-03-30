using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Manager, Student
    }
}