using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using TCSA.WebAPI.FlightData.Data;
using TCSA.WebAPI.FlightData.Models;
using TCSA.WebAPI.FlightData.Dtos;
using AutoMapper;

namespace TCSA.WebAPI.FlightData.Services;

public class FlightService : IFlightService
{
    private readonly FlightsDbContext _dbContext;
    private readonly IMapper _mapper;

    public FlightService(FlightsDbContext context, IMapper mapper) 
    {
        this._dbContext = context;
        this._mapper = mapper; 
    }

    public async Task<Flight> CreateFlight(FlightApiRequestDto flight)
    {
       Flight newFlight= _mapper.Map<Flight>(flight); // Use Mapper to map DTO to Flight entity
        var savedFlight = await _dbContext.Flights.AddAsync(newFlight);
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

    public async Task<Flight?> UpdateFlight(int id, FlightApiRequestDto updatedFlight)
    {
        Flight? savedFlight = await _dbContext.Flights.FindAsync(id);

        if (savedFlight is null)
        {
            return null;
        }

        savedFlight = _mapper.Map(updatedFlight, savedFlight); // Use Mapper to map DTO to Flight entity
        savedFlight.Id = id; // Ensure the ID is set correctly

        await _dbContext.SaveChangesAsync();

        return savedFlight;

    }

    public Task<Flight?> UpdateFlight(int id, Flight updatedFlight)
    {
        throw new NotImplementedException();
    }
}
