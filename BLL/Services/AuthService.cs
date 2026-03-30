using BLL.DTOs;
using DAL;
using DAL.EF.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly DataAccessFactory factory;
        private readonly string jwtSecret;
        private readonly string jwtIssuer;
        private readonly string jwtAudience;
        private readonly int jwtExpirationMinutes;

        public AuthService(DataAccessFactory factory, IConfiguration configuration)
        {
            this.factory = factory;
            jwtSecret = configuration["Jwt:Secret"] ?? "your-secret-key-at-least-32-characters-long";
            jwtIssuer = configuration["Jwt:Issuer"] ?? "YourApp";
            jwtAudience = configuration["Jwt:Audience"] ?? "YourAppUsers";
            jwtExpirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public (bool Success, string Message, UserDTO? User) Register(RegisterDTO dto)
        {
            try
            {
                // Validate email doesn't exist
                var existingUsers = factory.UserData().Get();
                if (existingUsers.Any(u => u.Email.ToLower() == dto.Email.ToLower()))
                {
                    return (false, "Email already exists", null);
                }

                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email.ToLower(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = dto.Role ?? "Student" // Default role
                };

                var created = factory.UserData().Create(user);
                if (created)
                {
                    var userDTO = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role
                    };
                    return (true, "Registration successful", userDTO);
                }

                return (false, "Registration failed", null);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Login user and return JWT token
        /// </summary>
        public (bool Success, string Token, UserDTO? User) Login(LoginDTO dto)
        {
            try
            {
                var users = factory.UserData().Get();
                var user = users.FirstOrDefault(u => u.Email.ToLower() == dto.Email.ToLower());

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    return (false, "Invalid email or password", null);
                }

                var token = GenerateJwtToken(user);
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                };

                return (true, token, userDTO);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Generate JWT token for authenticated user
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtExpirationMinutes),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Validate JWT token and extract claims
        /// </summary>
        public (bool Valid, ClaimsPrincipal? Principal) ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecret);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return (true, principal);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
