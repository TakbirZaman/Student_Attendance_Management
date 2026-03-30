namespace BLL.DTOs
{
    public class AttendanceDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; } // Present, Late, Absent, Leave
        public bool IsPresent { get; set; }
        public decimal Overtime { get; set; }
        public int CId { get; set; }
        public int SId { get; set; }
    }
}