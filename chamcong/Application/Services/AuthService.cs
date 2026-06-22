using chamcong.Application.Common;
using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace chamcong.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TokenResponseDto>> GenerateTokenAsync(LoginRequestDto request)
        {
            var hashedPwd = HashPassword(request.Password);

            // Verify against database
            var accounts = await _unitOfWork.Accounts.FindAsync(a => a.Username == request.Username && a.PasswordHash == hashedPwd);
            var account = accounts.FirstOrDefault();

            if (account == null)
            {
                return Result<TokenResponseDto>.Failure("Tài khoản hoặc mật khẩu không chính xác.", 401);
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("Secret");

            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.Name, account.Username),
                new System.Security.Claims.Claim(ClaimTypes.Role, account.Role)
            };

            if (account.EmployeeId.HasValue)
            {
                claims.Add(new System.Security.Claims.Claim("EmployeeId", account.EmployeeId.Value.ToString()));
            }

            // Keep WorkshopId logic if it's a partner
            if (account.Role == "Partner" && account.EmployeeId.HasValue)
            {
                var emp = await _unitOfWork.Employees.GetByIdAsync(account.EmployeeId.Value);
                if (emp != null)
                {
                    claims.Add(new System.Security.Claims.Claim("WorkshopId", emp.WorkshopId.ToString()));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpiryMinutes")),
                Issuer = jwtSettings.GetValue<string>("Issuer"),
                Audience = jwtSettings.GetValue<string>("Audience"),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var res = new TokenResponseDto
            {
                Token = tokenHandler.WriteToken(token)
            };

            return Result<TokenResponseDto>.Ok(res);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
