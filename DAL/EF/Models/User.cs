using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.EF.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "VARCHAR")]
        public string Role { get; set; } // Admin, Manager, Student
    }
}