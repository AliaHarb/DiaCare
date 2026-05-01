using DiaCare.Application.Interfaces;
using DiaCare.Domain.DTOS;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DiaCare.Application.Helpers;
using DiaCare.Domain.Entities;
using Microsoft.Extensions.Options;

namespace DiaCare.Application.Services
{
    public class ProfileService:BaseService, IProfileServices
    {
        private UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<UserProfileDto> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return _mapper.Map<UserProfileDto>(user); 
        }
    }
}
