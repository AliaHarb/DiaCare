using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int TotalPoints { get; set; } = 0;
    }
}
