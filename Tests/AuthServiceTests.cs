using Xunit;
using BLL.Services;
using BLL.DTOs;
using Moq;
using DAL;
using DAL.EF.Models;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    public class AuthServiceTests
    {
        private Mock<DataAccessFactory> GetMockFactory()
        {
            return new Mock<DataAccessFactory>(null);
        }

        private IConfiguration GetMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Jwt:Secret", "your-super-secret-key-that-is-at-least-32-characters-long-for-security"},
                {"Jwt:Issuer", "AttendanceAPI"},
                {"Jwt:Audience", "AttendanceAPIUsers"},
                {"Jwt:ExpirationMinutes", "60"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return configuration;
        }

        [Fact]
        public void Register_WithNewEmail_ReturnsSuccess()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var config = GetMockConfiguration();
            var service = new AuthService(mockFactory.Object, config);

            var registerDto = new RegisterDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "TestPassword123!",
                Role = "Student"
            };

            mockFactory.Setup(f => f.UserData().Get())
                .Returns(new List<User>());

            mockFactory.Setup(f => f.UserData().Create(It.IsAny<User>()))
                .Returns(true);

            // Act
            var (success, message, user) = service.Register(registerDto);

            // Assert
            Assert.True(success);
            Assert.NotNull(user);
            Assert.Equal("test@example.com", user.Email);
        }

        [Fact]
        public void Register_WithExistingEmail_ReturnsFail()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var config = GetMockConfiguration();
            var service = new AuthService(mockFactory.Object, config);

            var registerDto = new RegisterDTO
            {
                Name = "Test User",
                Email = "existing@example.com",
                Password = "TestPassword123!",
                Role = "Student"
            };

            var existingUsers = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Existing User",
                    Email = "existing@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                    Role = "Student"
                }
            };

            mockFactory.Setup(f => f.UserData().Get())
                .Returns(existingUsers);

            // Act
            var (success, message, user) = service.Register(registerDto);

            // Assert
            Assert.False(success);
            Assert.Null(user);
            Assert.Contains("already exists", message);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var config = GetMockConfiguration();
            var service = new AuthService(mockFactory.Object, config);

            var loginDto = new LoginDTO
            {
                Email = "user@example.com",
                Password = "TestPassword123!"
            };

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Test User",
                    Email = "user@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!"),
                    Role = "Student"
                }
            };

            mockFactory.Setup(f => f.UserData().Get())
                .Returns(users);

            // Act
            var (success, token, user) = service.Login(loginDto);

            // Assert
            Assert.True(success);
            Assert.NotEmpty(token);
            Assert.NotNull(user);
            Assert.Equal("user@example.com", user.Email);
        }

        [Fact]
        public void Login_WithInvalidPassword_ReturnsFail()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var config = GetMockConfiguration();
            var service = new AuthService(mockFactory.Object, config);

            var loginDto = new LoginDTO
            {
                Email = "user@example.com",
                Password = "WrongPassword"
            };

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Test User",
                    Email = "user@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!"),
                    Role = "Student"
                }
            };

            mockFactory.Setup(f => f.UserData().Get())
                .Returns(users);

            // Act
            var (success, token, user) = service.Login(loginDto);

            // Assert
            Assert.False(success);
            Assert.Null(user);
        }

        [Fact]
        public void Login_WithNonExistentEmail_ReturnsFail()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var config = GetMockConfiguration();
            var service = new AuthService(mockFactory.Object, config);

            var loginDto = new LoginDTO
            {
                Email = "nonexistent@example.com",
                Password = "TestPassword123!"
            };

            mockFactory.Setup(f => f.UserData().Get())
                .Returns(new List<User>());

            // Act
            var (success, token, user) = service.Login(loginDto);

            // Assert
            Assert.False(success);
            Assert.Null(user);
        }

        [Fact]
        public void ValidateToken_WithValidToken_ReturnsTrue()
        {
            // Arrange
            var mockFactory = GetMockFactory();
            var config = GetMockConfiguration();
            var service = new AuthService(mockFactory.Object, config);

            var testUser = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!"),
                Role = "Student"
            };

            mockFactory.Setup(f => f.UserData().Get())
                .Returns(new List<User> { testUser });

            // Generate a token
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "TestPassword123!" };
            var (_, token, _) = service.Login(loginDto);

            // Act
            var (valid, principal) = service.ValidateToken(token);

            // Assert
            Assert.True(valid);
            Assert.NotNull(principal);
        }
    }
}
