using BLL.DTOs;
using DAL;
using DAL.EF.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ReportService
    {
        private readonly DataAccessFactory factory;

        public ReportService(DataAccessFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Get attendance percentage for a student in a month
        /// </summary>
        public decimal GetAttendancePercentage(int studentId, int month, int year)
        {
            var attendances = factory.AttendanceData().Get();
            var studentAttendances = attendances
                .Where(a => a.SId == studentId &&
                           a.Date.Month == month &&
                           a.Date.Year == year)
                .ToList();

            if (studentAttendances.Count == 0)
                return 0;

            var presentCount = studentAttendances.Count(a => a.IsPresent);
            return Math.Round((decimal)presentCount / studentAttendances.Count * 100, 2);
        }

        /// <summary>
        /// Get summary for all students in a month
        /// </summary>
        public List<MonthlyReportDTO> GetMonthlyReport(int month, int year)
        {
            var attendances = factory.AttendanceData().Get();
            var students = factory.StudentData().Get();

            var report = students.Select(s => new MonthlyReportDTO
            {
                StudentId = s.Id,
                StudentName = s.Name,
                TotalDays = attendances.Count(a => a.SId == s.Id &&
                                                    a.Date.Month == month &&
                                                    a.Date.Year == year),
                PresentDays = attendances.Count(a => a.SId == s.Id &&
                                                      a.IsPresent &&
                                                      a.Date.Month == month &&
                                                      a.Date.Year == year),
                LateDays = attendances.Count(a => a.SId == s.Id &&
                                                   a.Status == "Late" &&
                                                   a.Date.Month == month &&
                                                   a.Date.Year == year),
                AbsentDays = attendances.Count(a => a.SId == s.Id &&
                                                     !a.IsPresent &&
                                                     a.Date.Month == month &&
                                                     a.Date.Year == year)
            }).ToList();

            // Calculate percentages
            foreach (var item in report)
            {
                item.AttendancePercentage = item.TotalDays > 0
                    ? Math.Round((decimal)item.PresentDays / item.TotalDays * 100, 2)
                    : 0;
            }

            return report;
        }

        /// <summary>
        /// Get students with late arrivals in a date range
        /// </summary>
        public List<LateStudentReportDTO> GetLateStudentsReport(DateTime startDate, DateTime endDate)
        {
            var attendances = factory.AttendanceData().Get();
            var students = factory.StudentData().Get();

            var lateAttendances = attendances
                .Where(a => a.Status == "Late" &&
                           a.Date >= startDate &&
                           a.Date <= endDate)
                .GroupBy(a => a.SId)
                .Select(g => new LateStudentReportDTO
                {
                    StudentId = g.Key,
                    StudentName = students.FirstOrDefault(s => s.Id == g.Key)?.Name ?? "Unknown",
                    LateCount = g.Count(),
                    LateDates = g.Select(a => a.Date).ToList()
                })
                .OrderByDescending(r => r.LateCount)
                .ToList();

            return lateAttendances;
        }

        /// <summary>
        /// Get class attendance summary
        /// </summary>
        public ClassAttendanceSummaryDTO GetClassAttendanceSummary(int classId, int month, int year)
        {
            var attendances = factory.AttendanceData().Get();
            var classData = factory.ClassData().Get(classId);

            var classAttendances = attendances
                .Where(a => a.CId == classId &&
                           a.Date.Month == month &&
                           a.Date.Year == year)
                .ToList();

            if (classData == null || classAttendances.Count == 0)
                return new ClassAttendanceSummaryDTO();

            return new ClassAttendanceSummaryDTO
            {
                ClassId = classId,
                ClassName = classData.Name,
                Month = month,
                Year = year,
                TotalRecords = classAttendances.Count,
                TotalPresent = classAttendances.Count(a => a.IsPresent),
                TotalAbsent = classAttendances.Count(a => !a.IsPresent),
                TotalLate = classAttendances.Count(a => a.Status == "Late"),
                TotalOvertime = classAttendances.Sum(a => a.Overtime),
                AverageAttendancePercentage = classAttendances.Count > 0
                    ? Math.Round((decimal)classAttendances.Count(a => a.IsPresent) / classAttendances.Count * 100, 2)
                    : 0
            };
        }

        /// <summary>
        /// Get students with lowest attendance
        /// </summary>
        public List<AttendanceAlertDTO> GetLowAttendanceStudents(int month, int year, decimal thresholdPercentage = 75)
        {
            var students = factory.StudentData().Get();
            var alerts = new List<AttendanceAlertDTO>();

            foreach (var student in students)
            {
                var percentage = GetAttendancePercentage(student.Id, month, year);

                if (percentage < thresholdPercentage)
                {
                    alerts.Add(new AttendanceAlertDTO
                    {
                        StudentId = student.Id,
                        StudentName = student.Name,
                        AttendancePercentage = percentage,
                        AlertLevel = percentage < 60 ? "Critical" : "Warning"
                    });
                }
            }

            return alerts.OrderBy(a => a.AttendancePercentage).ToList();
        }
    }

    // DTOs for Reports
    public class MonthlyReportDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int LateDays { get; set; }
        public int AbsentDays { get; set; }
        public decimal AttendancePercentage { get; set; }
    }

    public class LateStudentReportDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int LateCount { get; set; }
        public List<DateTime> LateDates { get; set; }
    }

    public class ClassAttendanceSummaryDTO
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLate { get; set; }
        public decimal TotalOvertime { get; set; }
        public decimal AverageAttendancePercentage { get; set; }
    }

    public class AttendanceAlertDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public decimal AttendancePercentage { get; set; }
        public string AlertLevel { get; set; } // Warning, Critical
    }
}
