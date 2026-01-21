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

        [Required]
        public bool IsPresent { get; set; }

        
        [ForeignKey("Student")]
        public int SId { get; set; }
        public virtual Student Student { get; set; }

    
        [ForeignKey("Class")]
        public int CId { get; set; }
        public virtual Class Class { get; set; }
    }
}
