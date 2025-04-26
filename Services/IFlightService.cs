using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Services;

public interface IFlightService
{
    public Task<List<Flight>> GetAllFlights();
    public Task<Flight?> GetFlightById(int id);
    public Task<Flight> CreateFlight(Flight flight);
    public Task<Flight?> UpdateFlight(int id, Flight updatedFlight);
    public Task<string?> DeleteFlight(int id);
}