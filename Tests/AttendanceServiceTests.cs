using Xunit;
using BLL.Services;
using BLL.DTOs;
using Moq;
using DAL;
using DAL.EF.Models;

namespace Tests
{
    public class AttendanceServiceTests
    {
        [Fact]
        public void Create_WithValidData_MarksPresentStudent_OnTime()
        {
            // Arrange
            var mockFactory = new Mock<DataAccessFactory>(null);
            var service = new AttendanceService(mockFactory.Object);

            var attendanceDto = new AttendanceDTO
            {
                Id = 1,
                Date = DateTime.Now,
                CheckInTime = DateTime.Now.Date.AddHours(8), // Before 9 AM
                IsPresent = true,
                SId = 1,
                CId = 1
            };

            mockFactory.Setup(f => f.AttendanceData().Create(It.IsAny<Attendance>()))
                .Returns(true);

            // Act
            var result = service.Create(attendanceDto);

            // Assert
            Assert.True(result);
            mockFactory.Verify(f => f.AttendanceData().Create(It.IsAny<Attendance>()), Times.Once);
        }

        [Fact]
        public void Create_WithCheckInAfter9AM_StatusIsLate()
        {
            // Arrange
            var mockFactory = new Mock<DataAccessFactory>(null);
            var service = new AttendanceService(mockFactory.Object);

            var attendanceDto = new AttendanceDTO
            {
                Id = 1,
                Date = DateTime.Now,
                CheckInTime = DateTime.Now.Date.AddHours(10), // After 9 AM
                IsPresent = true,
                SId = 1,
                CId = 1
            };

            mockFactory.Setup(f => f.AttendanceData().Create(It.IsAny<Attendance>()))
                .Callback<Attendance>(a =>
                {
                    Assert.Equal("Late", a.Status);
                })
                .Returns(true);

            // Act
            var result = service.Create(attendanceDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Create_WithAbsentStudent_StatusIsAbsent()
        {
            // Arrange
            var mockFactory = new Mock<DataAccessFactory>(null);
            var service = new AttendanceService(mockFactory.Object);

            var attendanceDto = new AttendanceDTO
            {
                Id = 1,
                Date = DateTime.Now,
                IsPresent = false,
                SId = 1,
                CId = 1
            };

            mockFactory.Setup(f => f.AttendanceData().Create(It.IsAny<Attendance>()))
                .Callback<Attendance>(a =>
                {
                    Assert.Equal("Absent", a.Status);
                })
                .Returns(true);

            // Act
            var result = service.Create(attendanceDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Create_WithCheckOutAfter5PM_CalculatesOvertime()
        {
            // Arrange
            var mockFactory = new Mock<DataAccessFactory>(null);
            var service = new AttendanceService(mockFactory.Object);

            var attendanceDto = new AttendanceDTO
            {
                Id = 1,
                Date = DateTime.Now,
                CheckInTime = DateTime.Now.Date.AddHours(9),
                CheckOutTime = DateTime.Now.Date.AddHours(18), // 1 hour after 5 PM
                IsPresent = true,
                SId = 1,
                CId = 1
            };

            mockFactory.Setup(f => f.AttendanceData().Create(It.IsAny<Attendance>()))
                .Callback<Attendance>(a =>
                {
                    Assert.True(a.Overtime > 0);
                    Assert.InRange(a.Overtime, 0.9m, 1.1m); // Approximately 1 hour
                })
                .Returns(true);

            // Act
            var result = service.Create(attendanceDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Get_ReturnsAllAttendance()
        {
            // Arrange
            var mockFactory = new Mock<DataAccessFactory>(null);
            var service = new AttendanceService(mockFactory.Object);

            var attendances = new List<Attendance>
            {
                new Attendance { Id = 1, Date = DateTime.Now, IsPresent = true },
                new Attendance { Id = 2, Date = DateTime.Now, IsPresent = false }
            };

            mockFactory.Setup(f => f.AttendanceData().Get())
                .Returns(attendances);

            // Act
            var result = service.Get();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Delete_WithValidId_ReturnsTrue()
        {
            // Arrange
            var mockFactory = new Mock<DataAccessFactory>(null);
            var service = new AttendanceService(mockFactory.Object);

            mockFactory.Setup(f => f.AttendanceData().Delete(1))
                .Returns(true);

            // Act
            var result = service.Delete(1);

            // Assert
            Assert.True(result);
            mockFactory.Verify(f => f.AttendanceData().Delete(1), Times.Once);
        }
    }
}
