using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiaCare.Application.Adapters;
using DiaCare.Domain.DTOS;
using System.Text.Json;
using System.Collections.Generic;

namespace DiaCare.UnitTests
{
    [TestClass]
    public class PredictionAdapterTests
    {
        private readonly PredictionAdapter _adapter = new PredictionAdapter();

        // Test Case 1
        
        /// <summary>
        /// Tests if specific fields in the DTO are correctly mapped to the AI request dictionary keys.
        /// Uses DataRow to avoid code duplication for similar mapping logic.
        /// </summary> 
        [DataTestMethod]
        [DataRow(27.5, "bmi")]
        [DataRow(1.0, "family_history_diabetes")]
        public void MapToAiRequest_MapsIndividualFields_Correctly(double value, string key)
        {
            var dto = new PredictionInputDto();
            if (key == "bmi") dto.Bmi = value;
            if (key == "family_history_diabetes") dto.FamilyHistoryDiabetes = (int)value;

            var result = _adapter.MapToAiRequest(dto) as Dictionary<string, object>;

            Assert.IsNotNull(result);
            Assert.AreEqual(value, double.Parse(result[key].ToString()));
        }

        // Test Case 2

        /// <summary>
        /// Verifies that the adapter maps all 12 required medical fields for the AI model.
        /// </summary>
        [TestMethod]
        public void MapToAiRequest_ReturnsDictionary_WithAllTwelveFields()
        {
            var dto = new PredictionInputDto
            {
                Bmi = 27.5, BloodPressure = 80, FastingGlucoseLevel = 110,
                InsulinLevel = 15, HbA1cLevel = 6.1, CholesterolLevel = 200,
                TriglyceridesLevel = 150, PhysicalActivityLevel = 2,
                DailyCalorieIntake = 2000, SugarIntakeGramsPerDay = 50,
                FamilyHistoryDiabetes = 0, WaistCircumferenceCm = 85
            };

            var result = _adapter.MapToAiRequest(dto) as Dictionary<string, object>;

            Assert.IsNotNull(result);
            Assert.AreEqual(12, result.Count);
        }

        // Test Case 3

        /// <summary>
        /// Validates that the AI JSON response (category and score) is correctly parsed into a DTO.
        /// Tested with multiple data rows to ensure consistency across different risk levels.
        /// </summary>
        [DataTestMethod]
        [DataRow("High", 75.5)]
        [DataRow("Low", 12.3)]
        [DataRow("Medium", 50.0)]
        public void MapFromAiResponse_ReturnsDto_WithCorrectValues(string category, double score)
        {
            var jsonString = $"{{\"risk_category\":\"{category}\",\"risk_score\":{score}}}";
            var json = JsonDocument.Parse(jsonString).RootElement;

            var result = _adapter.MapFromAiResponse(json);

            Assert.AreEqual(category, result.RiskCategory);
            Assert.AreEqual(score, result.RiskScore);
        }

        // Test Case 4

        /// <summary>
        /// Ensures the mapping method returns the specific DTO type required by the application.
        /// </summary>
        [TestMethod]
        public void MapFromAiResponse_ReturnsCorrectType()
        {
            var json = JsonDocument.Parse("{\"risk_category\":\"High\",\"risk_score\":90.0}").RootElement;

            var result = _adapter.MapFromAiResponse(json);

            Assert.IsInstanceOfType(result, typeof(PredictionResultDto));
        }
    }
}