using TCSA.WebAPI.FlightData.Data;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Services;

public interface IFlightService
{
    public List<Flight> GetFlights();
    public Flight? GetFlightById(int id);
    public Flight Createflight(Flight flight);
    public Flight Updateflight(Flight flight);
    public string? DeleteFlight(int id);
}

public class FlightService : IFlightService
{
    private readonly FlightsDbContext Context;

    public FlightService(FlightsDbContext context)
    {
        Context = context;
    }

    public Flight Createflight(Flight flight)
    {
        var savedFlight = Context.Flights.Add(flight);
        Context.SaveChanges();
        return savedFlight.Entity;
    }

    public string? DeleteFlight(int id)
    {
        Flight savedFlight = Context.Flights.Find(id);

        if (savedFlight == null)
        {
            return null;
        }

        Context.Flights.Remove(savedFlight);

        return $"Successfully deleted flight with id: {id}";
    }

    public Flight? GetFlightById(int id)
    {
        Flight savedFlight = Context.Flights.Find(id);
        return savedFlight == null ? null : savedFlight;
    }

    public List<Flight> GetFlights()
    {
        return Context.Flights.ToList();
    }

    public Flight Updateflight(Flight flight)
    {
        Flight savedFlight = Context.Flights.Find(flight.Id);

        if (savedFlight == null)
        {
            return null;
        }

        Context.Entry(savedFlight).CurrentValues.SetValues(flight);
        Context.SaveChanges();

        return savedFlight;
    }
}
