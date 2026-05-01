using AutoMapper;
using DiaCare.Application.Helpers;
using DiaCare.Application.Interfaces;
using DiaCare.Domain.DTOS;
using DiaCare.Domain.Entities;
using DiaCare.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiaCare.Application.Services
{
    public class AuthService :BaseService, IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper, IOptions<JwtSettings> jwt)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var authResult = new AuthResultDto();
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                authResult.Message = "Incorrect Email or Password.";
                return authResult;
            }

            var jwtToken = await GenerateJWTToken(user);

            authResult.IsAuthenticated = true;
            authResult.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authResult.ExpirationDate = jwtToken.ValidTo;
            authResult.UserName = user.UserName!;
            authResult.Message = "Logged in Successfully";

            var roles = await _userManager.GetRolesAsync(user);
            authResult.Role = roles.FirstOrDefault() ?? "Patient";

            return authResult;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is not null)
                return new AuthResultDto { Message = "Email already exists" };

            var newUser = _mapper.Map<ApplicationUser>(dto);
            newUser.UserName = dto.Email; // ASP.NET Identity

            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
                return new AuthResultDto { Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            await _userManager.AddToRoleAsync(newUser, "Patient");

            var jwtToken = await GenerateJWTToken(newUser);
            return new AuthResultDto
            {
                UserName = newUser.UserName!,
                IsAuthenticated = true,
                Role = "Patient",
                Message = "Registered Successfully",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                ExpirationDate = jwtToken.ValidTo
            };
        }

        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: creds
            );
        }
    }
}