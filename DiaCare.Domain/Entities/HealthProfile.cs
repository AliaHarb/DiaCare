using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Domain.Entities
{
    public class HealthProfile
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        // To Serve Ai Model (it Take subset of them )
        public double Bmi { get; set; }
        public int BloodPressure { get; set; }
        public int FastingGlucoseLevel { get; set; }
        public double InsulinLevel { get; set; }
        public double HbA1cLevel { get; set; }
        public int CholesterolLevel { get; set; }
        public int TriglyceridesLevel { get; set; }
        public int PhysicalActivityLevel { get; set; } // (1, 2, 3)
        public int DailyCalorieIntake { get; set; }
        public double SugarIntakeGramsPerDay { get; set; }
        public int FamilyHistoryDiabetes { get; set; } // (0, 1)
        public double WaistCircumferenceCm { get; set; }

        public double Weight { get; set; }
        public double Height { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //  Navigation
        public ApplicationUser User { get; set; }
        public ICollection<PredictionResult> PredictionResults { get; set; }= new List<PredictionResult>();

    }
}
