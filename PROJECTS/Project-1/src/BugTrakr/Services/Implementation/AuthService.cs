using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BugTrakr.Models;
using BugTrakr.DTOs;
using BugTrakr.Repositories;

namespace BugTrakr.Services;
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User?> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userRepository.GetUserByUsernameAsync(registerDto.Username) != null)
            {
                return null; // Username already exists
            }

            CreatePasswordHash(registerDto.Password, out string passwordHash, out string passwordSalt);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            return await _userRepository.AddUserAsync(user);
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null; // Invalid credentials
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            // Implementation...
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            // Implementation...
            var saltBytes = Convert.FromBase64String(storedSalt);
            using (var hmac = new System.Security.Cryptography.HMACSHA512(saltBytes))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var storedHashBytes = Convert.FromBase64String(storedHash);
                return computedHash.SequenceEqual(storedHashBytes);
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ?? string.Empty);

            if (string.IsNullOrEmpty(_configuration["JWT:Secret"]))
            {
                throw new InvalidOperationException("JWT Secret is not configured.");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
