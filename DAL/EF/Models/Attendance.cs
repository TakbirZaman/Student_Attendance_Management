using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.EF.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        [StringLength(20)]
        [Column(TypeName = "VARCHAR")]
        public string Status { get; set; } // Present, Late, Absent, Leave

        [Required]
        public bool IsPresent { get; set; }

        public decimal Overtime { get; set; } = 0; // in hours

        [ForeignKey("Student")]
        public int SId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Class")]
        public int CId { get; set; }
        public virtual Class Class { get; set; }
    }
}
