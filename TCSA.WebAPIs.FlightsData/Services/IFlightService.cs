using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPIs.FlightsData.Services;

public interface IFlightService
{
    public List<Flight> GetFlights();
    public Flight? GetFlightById(int id);
    public Flight CreateFlight(Flight flight);
    public Flight UpdateFlight(Flight flight);
    public String DeleteFlight(int id);
}
