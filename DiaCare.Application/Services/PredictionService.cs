using DiaCare.Application.Adapters;
using DiaCare.Application.Interfaces;
using DiaCare.Domain.DTOS;
using DiaCare.Domain.Entities;
using DiaCare.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;         // for mapping by mapper solve bad smell code reduntant code.
namespace DiaCare.Application.Services
{
        public class PredictionService : BaseService, IPredictionService
        {
            private readonly string? _baseUrl;
            private readonly HttpClient _httpClient;
            private readonly IPredictionAdapter _predictionAdapter;

          
            public PredictionService(
                HttpClient httpClient,
                IConfiguration configuration,
                IPredictionAdapter predictionAdapter,
                IUnitOfWork unitOfWork,
                IMapper mapper) // [PULL UP FIELD Applied]
            {
                _baseUrl = configuration["AISettings:BaseUrl"] ?? throw new Exception("AI Base URL is missing");
                _httpClient = httpClient;
                _predictionAdapter = predictionAdapter;
            }

            /*  Refactoring 1 :: Long Method Smell  -> Apply Extract Method Refactoring
             -------------------------------------------------------------------

            //Before Refactoring :: All in one method

           public async Task<PredictionResultDto> PredictAsync(PredictionInputDto inputdto, string? userId)
           {
               try
               {
                   // 1. Map to AI  **********
                   var ClientAiRequest = _predictionAdapter.MapToAiRequest(inputdto);

                   // 2. Call AI    ************
                   var ModelReq = await _httpClient.PostAsJsonAsync(_baseUrl, ClientAiRequest);

                   if (!ModelReq.IsSuccessStatusCode)
                   {
                       return new PredictionResultDto { RiskCategory = "AI Model Error" };
                   }

                   // 3. Read response  ********
                   var aiRawResponse = await ModelReq.Content.ReadFromJsonAsync<JsonElement>();

                   // 4. Map response  ********
                   var resultDto = _predictionAdapter.MapFromAiResponse(aiRawResponse);

                   // 5. Create HealthProfile  **********
                   var healthProfile = new HealthProfile
                   {
                       UserId = userId,

                       Bmi = inputdto.Bmi,
                       BloodPressure = inputdto.BloodPressure,
                       FastingGlucoseLevel = inputdto.FastingGlucoseLevel,
                       InsulinLevel = inputdto.InsulinLevel,
                       HbA1cLevel = inputdto.HbA1cLevel,
                       CholesterolLevel = inputdto.CholesterolLevel,
                       TriglyceridesLevel = inputdto.TriglyceridesLevel,
                       PhysicalActivityLevel = inputdto.PhysicalActivityLevel,
                       DailyCalorieIntake = inputdto.DailyCalorieIntake,
                       SugarIntakeGramsPerDay = inputdto.SugarIntakeGramsPerDay,
                       FamilyHistoryDiabetes = inputdto.FamilyHistoryDiabetes,
                       WaistCircumferenceCm = inputdto.WaistCircumferenceCm,

                       CreatedAt = DateTime.UtcNow
                   };

                    // 7. Save HealthProfile ***********
                    await _unitOfWork.HealthProfiles.AddAsync(healthProfile);

                    // 8. Create PredictionResult **********
                    var predictionRecord = new PredictionResult
                    {
                        UserId = userId,
                        HealthProfile = healthProfile, //  Navigation property
                        RiskScore = resultDto.RiskScore,
                        ResultText = resultDto.RiskCategory,
                        Recommendation = "Please follow up with a specialist for further advice.",
                        CreatedAt = DateTime.UtcNow
                    };
                    // 9. Save Prediction  ********
                    await _unitOfWork.PredictionResults.AddAsync(predictionRecord);
                    await _unitOfWork.SaveChangesAsync();

                    return resultDto;
                }
             */

            //After Refactoring ::
            //Extracted the database saving logic into a separate method SavePredictionDataAsync
            //for better readability and maintainability.
            public async Task<PredictionResultDto> PredictAsync(PredictionInputDto inputdto, string? userId)
        {
            try
            {
                // 1. Map to AI
                var ClientAiRequest = _predictionAdapter.MapToAiRequest(inputdto);

                // 2. Call AI
                var ModelReq = await _httpClient.PostAsJsonAsync(_baseUrl, ClientAiRequest);

                if (!ModelReq.IsSuccessStatusCode)
                {
                    return new PredictionResultDto { RiskCategory = "AI Model Error" };
                }

                // 3. Read response
                var aiRawResponse = await ModelReq.Content.ReadFromJsonAsync<JsonElement>();

                // 4. Map response
                var resultDto = _predictionAdapter.MapFromAiResponse(aiRawResponse);

                // 2. Apply Extract method
                await SavePredictionDataAsync(inputdto, resultDto, userId);

                return resultDto;
            }
            catch (Exception)
            {
                return new PredictionResultDto { RiskCategory = "Service unavailable" };
            }
        }

        //  (The Extracted Method) --- Save in database 
        private async Task SavePredictionDataAsync(PredictionInputDto inputdto, PredictionResultDto resultDto, string? userId)
        {
            /*
               ------------  Refactoring 2 -> Type ( duplicated )
               --------------------------------------------------------------

            //Before 

                var healthProfile = new HealthProfile
              {
                  UserId = userId,

                  Bmi = inputdto.Bmi,
                  BloodPressure = inputdto.BloodPressure,
                  FastingGlucoseLevel = inputdto.FastingGlucoseLevel,
                  InsulinLevel = inputdto.InsulinLevel,
                  HbA1cLevel = inputdto.HbA1cLevel,
                  CholesterolLevel = inputdto.CholesterolLevel,
                  TriglyceridesLevel = inputdto.TriglyceridesLevel,
                  PhysicalActivityLevel = inputdto.PhysicalActivityLevel,
                  DailyCalorieIntake = inputdto.DailyCalorieIntake,
                  SugarIntakeGramsPerDay = inputdto.SugarIntakeGramsPerDay,
                  FamilyHistoryDiabetes = inputdto.FamilyHistoryDiabetes,
                  WaistCircumferenceCm = inputdto.WaistCircumferenceCm,

                  CreatedAt = DateTime.UtcNow
              };
            */

            // After :: Using AutoMapper to map from inputdto to healthProfile
            var healthProfile = _mapper.Map<HealthProfile>(inputdto);
            healthProfile.UserId = userId;
            healthProfile.CreatedAt = DateTime.UtcNow;

            // 7. Save HealthProfile
            await _unitOfWork.HealthProfiles.AddAsync(healthProfile);

            // 8. Create PredictionResult
            var predictionRecord = new PredictionResult
            {
                UserId = userId,
                HealthProfile = healthProfile, 
                RiskScore = resultDto.RiskScore,
                ResultText = resultDto.RiskCategory,
                Recommendation = "Please follow up with a specialist for further advice.",
                CreatedAt = DateTime.UtcNow
            };
            // 9. Save Prediction
            await _unitOfWork.PredictionResults.AddAsync(predictionRecord);
            await _unitOfWork.SaveChangesAsync();
           
        }
    }
}




