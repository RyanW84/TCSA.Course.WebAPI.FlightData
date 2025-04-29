using AutoMapper;

using TCSA.WebAPI.FlightData.Dtos;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Flight, FlightApiRequestDto>();
        CreateMap<FlightApiRequestDto, Flight>();
    }
}