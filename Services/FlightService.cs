using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using TCSA.WebAPI.FlightData.Data;
using TCSA.WebAPI.FlightData.Models;
using TCSA.WebAPI.FlightData.Dtos;
using AutoMapper;
using System.Net;

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

    public async Task<ApiResponseDto<Flight>> CreateFlight(FlightApiRequestDto flight)
    {
       Flight newFlight= _mapper.Map<Flight>(flight); // Use Mapper to map DTO to Flight entity
        var savedFlight = await _dbContext.Flights.AddAsync(newFlight);
        await _dbContext.SaveChangesAsync();
        return new ApiResponseDto<Flight>
        {
            Data = savedFlight.Entity,
            ResponseCode = HttpStatusCode.Created,
        };
    }

    public async Task<ApiResponseDto<string?>> DeleteFlight(int id)
    {
        Flight? savedFlight = await _dbContext.Flights.FindAsync(id);

        if (savedFlight == null)
        {
            return new ApiResponseDto<string?>()
            {
                RequestFailed = true,
                Data = null,
                ResponseCode = HttpStatusCode.NotFound,
                ErrorMessage = $"Resource with ID: {id} was not found",
            };
        }
        _dbContext.Flights.Remove(savedFlight);

        await _dbContext.SaveChangesAsync();

        return new ApiResponseDto<string?>()
        {
            Data = null,
            ResponseCode = HttpStatusCode.NoContent,
        };
    }

    public async Task<ApiResponseDto<List<Flight>>> GetAllFlights()
    {

        var flights = await _dbContext.Flights.ToListAsync();
        return new ApiResponseDto<List<Flight>>
        {
            Data = flights,
            ResponseCode = HttpStatusCode.OK
        };
    }

    public async Task<ApiResponseDto<Flight?>> GetFlightById(int id)
    {
        var result = await _dbContext.Flights.FindAsync(id);

       if (result is null)
        {
            return new ApiResponseDto<Flight?>()
            {
                RequestFailed = true,
                Data = null,
                ResponseCode = HttpStatusCode.NotFound,
                ErrorMessage = $"Resource with ID: {id} was not found",
            };
        }

       return new ApiResponseDto<Flight?>()
       {
           Data = result,
           ResponseCode = HttpStatusCode.OK,
       };
    }

    public async Task<ApiResponseDto<Flight?>> UpdateFlight(int id, FlightApiRequestDto updatedFlight)
    {
        Flight? savedFlight = await _dbContext.Flights.FindAsync(id);

        if (savedFlight is null)
        {
            return new ApiResponseDto<Flight?>()
            {
                RequestFailed = true,
                Data = null,
                ResponseCode = HttpStatusCode.NotFound,
                ErrorMessage = $"Resource with ID: {id} was not found",
            };
        }

        savedFlight = _mapper.Map(updatedFlight, savedFlight); // Use Mapper to map DTO to Flight entity
        savedFlight.Id = id; // Ensure the ID is set correctly

        await _dbContext.SaveChangesAsync();

        return new ApiResponseDto<Flight?>()
        {
            Data = savedFlight,
            ResponseCode = HttpStatusCode.OK,
        };
    }

    public Task<Flight?> UpdateFlight(int id, Flight updatedFlight)
    {
        throw new NotImplementedException();
    }
}
