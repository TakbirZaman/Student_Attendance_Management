using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.EF.Models
{
    public class Student
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        public string Email { get; set; }

        public virtual List<Attendance> Attendances { get; set; }

        public Student()
        {
            Attendances = new List<Attendance>();
        }
    }
}
