using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int TotalPoints { get; set; } = 0;

        //  Navigation Properties
        // Navigation Properties
        public ICollection<HealthProfile> HealthProfiles { get; set; } = new List<HealthProfile>();
        public ICollection<PredictionResult> PredictionResults { get; set; } = new List<PredictionResult>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    }
    }
