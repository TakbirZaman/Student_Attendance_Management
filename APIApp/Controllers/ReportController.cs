using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace APIApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly ReportService reportService;

        public ReportController(ReportService reportService)
        {
            this.reportService = reportService;
        }

        /// <summary>
        /// Get monthly attendance report for all students
        /// </summary>
        [HttpGet("monthly")]
        public ActionResult GetMonthlyReport(int month = 0, int year = 0)
        {
            if (month == 0) month = DateTime.Now.Month;
            if (year == 0) year = DateTime.Now.Year;

            if (month < 1 || month > 12)
                return BadRequest(new { message = "Month must be between 1 and 12" });

            var report = reportService.GetMonthlyReport(month, year);
            return Ok(new
            {
                month,
                year,
                data = report
            });
        }

        /// <summary>
        /// Get late students report for a date range
        /// </summary>
        [HttpGet("late-students")]
        public ActionResult GetLateStudents(DateTime? startDate = null, DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.Now.AddMonths(-1);
            var end = endDate ?? DateTime.Now;

            if (start > end)
                return BadRequest(new { message = "Start date must be before end date" });

            var report = reportService.GetLateStudentsReport(start, end);
            return Ok(new
            {
                startDate = start,
                endDate = end,
                totalLateStudents = report.Count,
                data = report
            });
        }

        /// <summary>
        /// Get attendance percentage for a specific student
        /// </summary>
        [HttpGet("student-percentage")]
        public ActionResult GetStudentAttendancePercentage(int studentId, int month = 0, int year = 0)
        {
            if (month == 0) month = DateTime.Now.Month;
            if (year == 0) year = DateTime.Now.Year;

            var percentage = reportService.GetAttendancePercentage(studentId, month, year);
            return Ok(new
            {
                studentId,
                month,
                year,
                attendancePercentage = percentage
            });
        }

        /// <summary>
        /// Get class attendance summary
        /// </summary>
        [HttpGet("class-summary")]
        public ActionResult GetClassSummary(int classId, int month = 0, int year = 0)
        {
            if (month == 0) month = DateTime.Now.Month;
            if (year == 0) year = DateTime.Now.Year;

            var summary = reportService.GetClassAttendanceSummary(classId, month, year);
            return Ok(summary);
        }

        /// <summary>
        /// Get students with low attendance (alert system)
        /// </summary>
        [HttpGet("low-attendance")]
        public ActionResult GetLowAttendanceStudents(int month = 0, int year = 0, decimal threshold = 75)
        {
            if (month == 0) month = DateTime.Now.Month;
            if (year == 0) year = DateTime.Now.Year;

            var alerts = reportService.GetLowAttendanceStudents(month, year, threshold);
            return Ok(new
            {
                month,
                year,
                threshold,
                alertCount = alerts.Count,
                data = alerts
            });
        }
    }
}
