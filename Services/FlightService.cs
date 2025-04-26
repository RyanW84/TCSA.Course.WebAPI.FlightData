using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using TCSA.WebAPI.FlightData.Data;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Services;

public class FlightService : IFlightService
{
    private readonly FlightsDbContext _dbContext;

    public FlightService(FlightsDbContext context)
    {
        this._dbContext = context;
    }

    public async Task<Flight> CreateFlight(Flight flight)
    {
        var savedFlight = await _dbContext.Flights.AddAsync(flight);
        await _dbContext.SaveChangesAsync();
        return savedFlight.Entity;
    }

    public async Task<string?> DeleteFlight(int id)
    {
        Flight? savedFlight = await _dbContext.Flights.FindAsync(id);

        if (savedFlight == null)
        {
            return "Flight not found."; // Avoid null return to fix CS8603
        }

        _dbContext.Flights.Remove(savedFlight);

        await _dbContext.SaveChangesAsync();

        return $"Successfully deleted flight with id: {id}";
    }

    public async Task<List<Flight>> GetAllFlights()
    {
        return await _dbContext.Flights.ToListAsync();
    }

    public async Task<Flight?> GetFlightById(int id)
    {
        var result = await _dbContext.Flights.FindAsync(id);

        return result; 
    }

    public async Task<Flight?> UpdateFlight(int id, Flight updatedFlight)
    {
        Flight? savedFlight = await _dbContext.Flights.FindAsync(id);

        if (savedFlight is null)
        {
            return null;
        }

        savedFlight.FlightNumber = updatedFlight.FlightNumber;
        savedFlight.AirlineName = updatedFlight.AirlineName;
        savedFlight.DepartureAirportCode = updatedFlight.DepartureAirportCode;
        savedFlight.ArrivalAirportCode = updatedFlight.ArrivalAirportCode;
        savedFlight.DepartureDateTime = updatedFlight.DepartureDateTime;
        savedFlight.ArrivalDateTime = updatedFlight.ArrivalDateTime;
        savedFlight.PassengerCapacity = updatedFlight.PassengerCapacity;

        await _dbContext.SaveChangesAsync();

        return savedFlight;

    }

}
