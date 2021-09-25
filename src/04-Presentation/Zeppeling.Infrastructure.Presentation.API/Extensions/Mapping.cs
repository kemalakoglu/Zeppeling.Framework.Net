using System;
using Zeppeling.Infrastructure.Domain.Contract.DTO.RC;
using AutoMapper;
using Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes;

namespace Zeppeling.Infrastructure.Presentation.API.Extensions
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<RC, GetRCByIdResponseDTO>();
        }
    }
}