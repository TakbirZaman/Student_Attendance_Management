using Xunit;
using BLL.Services;
using Moq;
using DAL;
using DAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class ReportServiceTests
    {
        private Mock<DataAccessFactory> GetMockFactory()
        {
            return new Mock<DataAccessFactory>(null);
        }

        [Fact]
        public void GetAttendancePercentage_CalculatesCorrectly()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var service = new ReportService(mockFactory.Object);

            var now = DateTime.Now;
            var attendances = new List<Attendance>
            {
                new Attendance { Id = 1, SId = 1, Date = now.Date.AddDays(-5), IsPresent = true },
                new Attendance { Id = 2, SId = 1, Date = now.Date.AddDays(-4), IsPresent = true },
                new Attendance { Id = 3, SId = 1, Date = now.Date.AddDays(-3), IsPresent = false },
                new Attendance { Id = 4, SId = 1, Date = now.Date.AddDays(-2), IsPresent = true },
                new Attendance { Id = 5, SId = 1, Date = now.Date.AddDays(-1), IsPresent = true }
            };

            mockFactory.Setup(f => f.AttendanceData().Get())
                .Returns(attendances);

            // Act
            var percentage = service.GetAttendancePercentage(1, now.Month, now.Year);

            // Assert
            Assert.Equal(80m, percentage); // 4 out of 5 = 80%
        }

        [Fact]
        public void GetAttendancePercentage_WithNoRecords_ReturnsZero()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var service = new ReportService(mockFactory.Object);

            mockFactory.Setup(f => f.AttendanceData().Get())
                .Returns(new List<Attendance>());

            // Act
            var percentage = service.GetAttendancePercentage(999, 1, 2024);

            // Assert
            Assert.Equal(0, percentage);
        }

        [Fact]
        public void GetMonthlyReport_ReturnsCorrectSummary()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var service = new ReportService(mockFactory.Object);

            var now = DateTime.Now;
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Student 1", RollNo = "001" },
                new Student { Id = 2, Name = "Student 2", RollNo = "002" }
            };

            var attendances = new List<Attendance>
            {
                new Attendance { Id = 1, SId = 1, Date = now.Date, IsPresent = true, Status = "Present" },
                new Attendance { Id = 2, SId = 1, Date = now.Date.AddDays(-1), IsPresent = false, Status = "Absent" },
                new Attendance { Id = 3, SId = 2, Date = now.Date, IsPresent = true, Status = "Late" },
                new Attendance { Id = 4, SId = 2, Date = now.Date.AddDays(-1), IsPresent = true, Status = "Present" }
            };

            mockFactory.Setup(f => f.AttendanceData().Get())
                .Returns(attendances);
            mockFactory.Setup(f => f.StudentData().Get())
                .Returns(students);

            // Act
            var report = service.GetMonthlyReport(now.Month, now.Year);

            // Assert
            Assert.NotNull(report);
            Assert.Equal(2, report.Count);
            Assert.True(report[0].TotalDays > 0);
        }

        [Fact]
        public void GetLateStudentsReport_ReturnsStudentsWithLateArrivals()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var service = new ReportService(mockFactory.Object);

            var startDate = DateTime.Now.Date.AddDays(-10);
            var endDate = DateTime.Now.Date;

            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Late Student", RollNo = "001" },
                new Student { Id = 2, Name = "Punctual Student", RollNo = "002" }
            };

            var attendances = new List<Attendance>
            {
                new Attendance { Id = 1, SId = 1, Date = startDate.AddDays(1), Status = "Late", IsPresent = true },
                new Attendance { Id = 2, SId = 1, Date = startDate.AddDays(2), Status = "Late", IsPresent = true },
                new Attendance { Id = 3, SId = 2, Date = startDate.AddDays(1), Status = "Present", IsPresent = true }
            };

            mockFactory.Setup(f => f.AttendanceData().Get())
                .Returns(attendances);
            mockFactory.Setup(f => f.StudentData().Get())
                .Returns(students);

            // Act
            var report = service.GetLateStudentsReport(startDate, endDate);

            // Assert
            Assert.NotNull(report);
            Assert.Single(report); // Only one student with late arrivals
            Assert.Equal(1, report[0].StudentId);
            Assert.Equal(2, report[0].LateCount);
        }

        [Fact]
        public void GetClassAttendanceSummary_CalculatesCorrectly()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var service = new ReportService(mockFactory.Object);

            var now = DateTime.Now;
            var classData = new Class { Id = 1, Name = "Class A" };

            var attendances = new List<Attendance>
            {
                new Attendance { Id = 1, CId = 1, Date = now.Date, IsPresent = true, Status = "Present", Overtime = 0.5m },
                new Attendance { Id = 2, CId = 1, Date = now.Date.AddDays(-1), IsPresent = true, Status = "Late", Overtime = 0 },
                new Attendance { Id = 3, CId = 1, Date = now.Date.AddDays(-2), IsPresent = false, Status = "Absent", Overtime = 0 }
            };

            mockFactory.Setup(f => f.ClassData().Get(1))
                .Returns(classData);
            mockFactory.Setup(f => f.AttendanceData().Get())
                .Returns(attendances);

            // Act
            var summary = service.GetClassAttendanceSummary(1, now.Month, now.Year);

            // Assert
            Assert.NotNull(summary);
            Assert.Equal("Class A", summary.ClassName);
            Assert.Equal(3, summary.TotalRecords);
            Assert.Equal(2, summary.TotalPresent);
            Assert.Equal(1, summary.TotalAbsent);
            Assert.Equal(1, summary.TotalLate);
            Assert.Equal(0.5m, summary.TotalOvertime);
        }

        [Fact]
        public void GetLowAttendanceStudents_ReturnsStudentsBelowThreshold()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var service = new ReportService(mockFactory.Object);

            var now = DateTime.Now;
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Low Attendance", RollNo = "001" },
                new Student { Id = 2, Name = "Good Attendance", RollNo = "002" }
            };

            var attendances = new List<Attendance>
            {
                // Student 1: 1 present out of 2 = 50%
                new Attendance { Id = 1, SId = 1, Date = now.Date, IsPresent = true },
                new Attendance { Id = 2, SId = 1, Date = now.Date.AddDays(-1), IsPresent = false },
                // Student 2: 4 present out of 5 = 80%
                new Attendance { Id = 3, SId = 2, Date = now.Date, IsPresent = true },
                new Attendance { Id = 4, SId = 2, Date = now.Date.AddDays(-1), IsPresent = true },
                new Attendance { Id = 5, SId = 2, Date = now.Date.AddDays(-2), IsPresent = true },
                new Attendance { Id = 6, SId = 2, Date = now.Date.AddDays(-3), IsPresent = true },
                new Attendance { Id = 7, SId = 2, Date = now.Date.AddDays(-4), IsPresent = false }
            };

            mockFactory.Setup(f => f.StudentData().Get())
                .Returns(students);
            mockFactory.Setup(f => f.AttendanceData().Get())
                .Returns(attendances);

            // Act
            var alerts = service.GetLowAttendanceStudents(now.Month, now.Year, 75);

            // Assert
            Assert.NotNull(alerts);
            Assert.Single(alerts); // Only Student 1 is below 75%
            Assert.Equal("Low Attendance", alerts[0].StudentName);
            Assert.Equal("Critical", alerts[0].AlertLevel); // 50% < 60%
        }
    }
}
