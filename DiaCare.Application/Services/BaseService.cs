using AutoMapper;
using DiaCare.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.Application.Services
{
    public abstract class BaseService //Refactoring Type: Pull Up Field  

    {
        protected  IUnitOfWork _unitOfWork;
        protected  IMapper _mapper;

        protected BaseService(IUnitOfWork ? unitOfWork = null, IMapper ? mapper=null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
