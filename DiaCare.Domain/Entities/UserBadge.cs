using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.Entities
{
    public class UserBadge
    {
        public string UserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.Now;

        //  Navigation
        public ApplicationUser User { get; set; }
        public Badge Badge { get; set; }
    }
}
