using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Services;

public interface IFlightService
{
    public Task<List<Flight>> GetAllFlights();
    public Task<List<Flight>> GetFlightById(int id);
    public Task<List<Flight>> Createflight(Flight flight);
    public Task<List<Flight>> GUpdateflight(int id,Flight updatedFlight);
    public Task<string?> DeleteFlight(int id);
}