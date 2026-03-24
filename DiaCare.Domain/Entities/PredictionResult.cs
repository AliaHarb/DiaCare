using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.Entities
{
   public class PredictionResult
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int HealthProfileId { get; set; }
        public double RiskScore { get; set; }
        public string ResultText { get; set; }
        public string Recommendation { get; set; } // Model Advice
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //  Navigation
        public ApplicationUser User { get; set; }
        public HealthProfile HealthProfile { get; set; }
    }
}
