using TCSA.WebAPI.FlightData.Dtos;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Services;

public interface IFlightService
    {
    public Task<ApiResponseDto<List<Flight>>> GetAllFlights(FlightFilterOptions filterOptions);
    public Task<ApiResponseDto<Flight?>> GetFlightById(int id);
    public Task<ApiResponseDto<Flight>> CreateFlight(FlightApiRequestDto flight);
    public Task<ApiResponseDto<Flight?>> UpdateFlight(int id , FlightApiRequestDto updatedFlight);
    public Task<ApiResponseDto<string?>> DeleteFlight(int id);
    }