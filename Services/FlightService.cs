using System.Net;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TCSA.WebAPI.FlightData.Data;
using TCSA.WebAPI.FlightData.Dtos;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Services;

public class FlightService: IFlightService
    {
    private readonly FlightsDbContext _dbContext;
    private readonly IMapper _mapper;

    public FlightService(FlightsDbContext context , IMapper mapper)
        {
        this._dbContext = context;
        this._mapper = mapper;
        }

    public async Task<ApiResponseDto<Flight>> CreateFlight(FlightApiRequestDto flight)
        {
        Flight newFlight = _mapper.Map<Flight>(flight); // Use Mapper to map DTO to Flight entity
        var savedFlight = await _dbContext.Flights.AddAsync(newFlight);
        await _dbContext.SaveChangesAsync();
        return new ApiResponseDto<Flight>
            {
            Data = savedFlight.Entity ,
            ResponseCode = HttpStatusCode.Created ,
            };
        }

    public async Task<ApiResponseDto<string?>> DeleteFlight(int id)
        {
        Flight? savedFlight = await _dbContext.Flights.FindAsync(id);

        if(savedFlight == null)
            {
            return new ApiResponseDto<string?>()
                {
                RequestFailed = true ,
                Data = null ,
                ResponseCode = HttpStatusCode.NotFound ,
                ErrorMessage = $"Resource with ID: {id} was not found" ,
                };
            }
        _dbContext.Flights.Remove(savedFlight);

        await _dbContext.SaveChangesAsync();

        return new ApiResponseDto<string?>()
            {
            Data = null ,
            ResponseCode = HttpStatusCode.NoContent ,
            };
        }

    public async Task<ApiResponseDto<List<Flight>>> GetAllFlights(FlightFilterOptions filterOptions)
        {
        var query = _dbContext.Flights.AsQueryable();
        // allows for expandable filtering and the queries are stackable

        if(!string.IsNullOrWhiteSpace(filterOptions.AirlineName))
            {
            query = query.Where(f => f.AirlineName.Contains(filterOptions.AirlineName)); // Filter by AirlineName
            }

        if(!string.IsNullOrEmpty(filterOptions.DepartureAirportCode))
            {
            query = query.Where(f => f.DepartureAirportCode.Contains(filterOptions.DepartureAirportCode)); // Filter by DepartureAirportCode
            }

        if(!string.IsNullOrEmpty(filterOptions.ArrivalAirportCode))
            {
            query = query.Where(f => f.ArrivalAirportCode.Contains(filterOptions.ArrivalAirportCode)); // Filter by ArrivalAirportCode
            }

        if(filterOptions.DepartureDateTime.HasValue) // Not a string
            {
            query = query.Where(f => f.DepartureDateTime.Date <= filterOptions.DepartureDateTime.Value.Date); // Filter by DepartureDateTime
            }
        if(filterOptions.ArrivalDateTime.HasValue) // Not a string
            {
            query = query.Where(f => f.ArrivalDateTime.Date <= filterOptions.ArrivalDateTime.Value.Date); // Filter by DepartureDateTime
            }

        var flights = await query.ToListAsync(); // Execute the query and get the results

        return new ApiResponseDto<List<Flight>>
            {
            Data = flights ,
            ResponseCode = HttpStatusCode.OK
            };
        }

    public async Task<ApiResponseDto<Flight?>> GetFlightById(int id)
        {
        var result = await _dbContext.Flights.FindAsync(id);

        if(result is null)
            {
            return new ApiResponseDto<Flight?>()
                {
                RequestFailed = true ,
                Data = null ,
                ResponseCode = HttpStatusCode.NotFound ,
                ErrorMessage = $"Resource with ID: {id} was not found" ,
                };
            }

        return new ApiResponseDto<Flight?>()
            {
            Data = result ,
            ResponseCode = HttpStatusCode.OK ,
            };
        }

    public async Task<ApiResponseDto<Flight?>> UpdateFlight(int id , FlightApiRequestDto updatedFlight)
        {
        Flight? savedFlight = await _dbContext.Flights.FindAsync(id);

        if(savedFlight is null)
            {
            return new ApiResponseDto<Flight?>()
                {
                RequestFailed = true ,
                Data = null ,
                ResponseCode = HttpStatusCode.NotFound ,
                ErrorMessage = $"Resource with ID: {id} was not found" ,
                };
            }

        savedFlight = _mapper.Map(updatedFlight , savedFlight); // Use Mapper to map DTO to Flight entity
        savedFlight.Id = id; // Ensure the ID is set correctly

        await _dbContext.SaveChangesAsync();

        return new ApiResponseDto<Flight?>()
            {
            Data = savedFlight ,
            ResponseCode = HttpStatusCode.OK ,
            };
        }

    public Task<Flight?> UpdateFlight(int id , Flight updatedFlight)
        {
        throw new NotImplementedException();
        }
    }
