using DiaCare.Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Application.Interfaces
{
    public interface IPredictionService
    {
        Task<PredictionResultDto> PredictAsync(PredictionInputDto inputdto, string? userId);

    }
}
