using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class StudentAttendanceDTO : StudentDTO
    {
       public List<AttendanceDTO> Attendances { get; set; }

        public StudentAttendanceDTO()
        {
            Attendances = new List<AttendanceDTO>();
        }
    }
}
