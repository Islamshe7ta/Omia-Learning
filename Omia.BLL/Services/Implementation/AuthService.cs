using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Omia.BLL.DTOs.Auth;
using Omia.BLL.Services.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly OmiaDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService(OmiaDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<LoginResponseDTO> Login(string username, string password)
        {
            var user = await _context.Set<BaseUserEntity>().FirstOrDefaultAsync(x => x.Username == username);

           
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new LoginResponseDTO { IsSuccess = false, Message = "اسم المستخدم أو كلمة المرور غير صحيحة" };
            }

            
            if (user.Status == AccountStatus.Paused)
            {
                string dateStr = user.PausedUntil?.ToString("yyyy-MM-dd") ?? "موعد غير محدد";
                return new LoginResponseDTO { IsSuccess = false, Message = $"الحساب متوقف مؤقتاً حتى {dateStr}" };
            }

            if (user.Status == AccountStatus.Deleted || user.IsDeleted)
            {
                return new LoginResponseDTO { IsSuccess = false, Message = "الحساب غير نشط (محذوف)" };
            }

            
            string role = user switch
            {
                Admin => "Admin",
                Teacher => "Teacher",
                Student => "Student",
                Assistant => "Assistant",
                Parent => "Parent",
                Institute => "Institute",
                _ => "User"
            };

            var token = GenerateJwtToken(user.Id.ToString(), role);
            var response = new LoginResponseDTO
            {
                IsSuccess = true,
                Token = token,
                UserType = role,
                Message = "تم تسجيل الدخول بنجاح"
            };

            if (user is Student student)
            {
                response.StudentUserProfile = _mapper.Map<StudentProfileDTO>(student);
            }
            else if (user is Parent parent)
            {
                response.ParentUserProfile = _mapper.Map<ParentUserProfileDTO>(parent);
            }
            else if (user is Teacher teacher)
            {
                response.TeacherUserProfile = _mapper.Map<TeacherProfileDTO>(teacher);
            }
            else if (user is Assistant assistant)
            {
                response.AssistantUserProfile = _mapper.Map<AssistantProfileDTO>(assistant);
            }
            else if (user is Institute institute)
            {
                response.InstituteUserProfile = _mapper.Map<InstituteProfileDTO>(institute);
            }

            return response;
        }



        
        private string GenerateJwtToken(string userId, string role)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var keyStr = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing in configuration.");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyStr)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(
                    double.Parse(jwtSettings["DurationInDays"] ?? "7")
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
