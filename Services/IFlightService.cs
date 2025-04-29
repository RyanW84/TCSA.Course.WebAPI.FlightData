using TCSA.WebAPI.FlightData.Dtos;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Services;

public interface IFlightService
{
    public Task<List<Flight>> GetAllFlights();
    public Task<Flight?> GetFlightById(int id);
    public Task<Flight> CreateFlight(FlightApiRequestDto flight);
    public Task<Flight?> UpdateFlight(int id, FlightApiRequestDto updatedFlight);
    public Task<string?> DeleteFlight(int id);
}