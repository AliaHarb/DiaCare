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
        public float Bmi { get; set; }
        public int BloodPressure { get; set; }
        public int FastingGlucoseLevel { get; set; }
        public float InsulinLevel { get; set; }
        public float HbA1cLevel { get; set; }
        public int CholesterolLevel { get; set; }
        public int TriglyceridesLevel { get; set; }
        public int PhysicalActivityLevel { get; set; } // (1, 2, 3)
        public int DailyCalorieIntake { get; set; }
        public float SugarIntakeGramsPerDay { get; set; }
        public int FamilyHistoryDiabetes { get; set; } // (0, 1)
        public float WaistCircumferenceCm { get; set; }

        public float Weight { get; set; }
        public float Height { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
