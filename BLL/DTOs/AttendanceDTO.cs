
namespace BLL.DTOs
{
    public class AttendanceDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }


        public int CId { get; set; }
        public int SId { get; set; }
    }
}