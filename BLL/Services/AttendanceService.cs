using BLL.DTOs;
using DAL;
using DAL.EF.Models;
using System.Collections.Generic;

namespace BLL.Services
{
    public class AttendanceService
    {
        DataAccessFactory factory;
        private const int SHIFT_START_HOUR = 9; // 9 AM
        private const int SHIFT_END_HOUR = 17; // 5 PM

        public AttendanceService(DataAccessFactory factory)
        {
            this.factory = factory;
        }

        public List<AttendanceDTO> Get()
        {
            var data = factory.AttendanceData().Get();
            var mapper = MapperConfig.GetMapper();
            var ret = mapper.Map<List<AttendanceDTO>>(data);
            return ret;
        }

        public AttendanceDTO Get(int id)
        {
            return MapperConfig.GetMapper().Map<AttendanceDTO>(factory.AttendanceData().Get(id));
        }

        public bool Create(AttendanceDTO a)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Attendance>(a);

            // Calculate status and overtime
            CalculateAttendanceStatus(data);

            return factory.AttendanceData().Create(data);
        }

        public bool Update(AttendanceDTO a)
        {
            var mapper = MapperConfig.GetMapper();
            var data = mapper.Map<Attendance>(a);

            // Recalculate status and overtime
            CalculateAttendanceStatus(data);

            return factory.AttendanceData().Update(data);
        }

        public bool Delete(int id)
        {
            return factory.AttendanceData().Delete(id);
        }

        /// <summary>
        /// Calculate attendance status (Present/Late/Absent) and overtime hours
        /// </summary>
        private void CalculateAttendanceStatus(Attendance attendance)
        {
            if (!attendance.IsPresent)
            {
                attendance.Status = "Absent";
                attendance.Overtime = 0;
                return;
            }

            // If no check-in time provided, mark as present (assume on time)
            if (!attendance.CheckInTime.HasValue)
            {
                attendance.Status = "Present";
                attendance.Overtime = 0;
                return;
            }

            var checkInHour = attendance.CheckInTime.Value.Hour;
            var checkInMinute = attendance.CheckInTime.Value.Minute;

            // Check if late (after shift start)
            if (checkInHour > SHIFT_START_HOUR ||
                (checkInHour == SHIFT_START_HOUR && checkInMinute > 0))
            {
                attendance.Status = "Late";
            }
            else
            {
                attendance.Status = "Present";
            }

            // Calculate overtime if checkout time is provided
            if (attendance.CheckOutTime.HasValue)
            {
                var checkOutHour = attendance.CheckOutTime.Value.Hour;
                var checkOutMinute = attendance.CheckOutTime.Value.Minute;

                if (checkOutHour > SHIFT_END_HOUR ||
                    (checkOutHour == SHIFT_END_HOUR && checkOutMinute > 0))
                {
                    // Calculate overtime in hours
                    var shiftEndTime = attendance.CheckOutTime.Value.Date
                        .AddHours(SHIFT_END_HOUR);
                    var overtimeSpan = attendance.CheckOutTime.Value - shiftEndTime;
                    attendance.Overtime = (decimal)overtimeSpan.TotalHours;
                }
            }
        }
    }
}
