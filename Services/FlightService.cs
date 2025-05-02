using AutoMapper;

using Microsoft.EntityFrameworkCore;

using System.Net;

using TCSA.WebAPI.FlightData.Data;
using TCSA.WebAPI.FlightData.Dtos;
using TCSA.WebAPI.FlightData.Models;

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
        Flight newFlight = _mapper.Map<Flight>(flight); // Use Mapper to map DTO to Flight entity
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

    public async Task<ApiResponseDto<List<Flight>>> GetAllFlights(FlightOptions flightOptions)
    {
        var query = _dbContext.Flights.Include(f => f.Airline).AsQueryable();
        var totalFlights = await query.CountAsync();
        // allows for? expandable filtering and the queries are stackable

        List<Flight>? flights;

        if (!string.IsNullOrWhiteSpace(flightOptions.AirlineName))
        {
            query = query.Where(f => f.Airline.Name.Contains(flightOptions.AirlineName)); // Filter by AirlineName
        }

        if (!string.IsNullOrEmpty(flightOptions.DepartureAirportCode))
        {
            query = query.Where(f => f.DepartureAirportCode.Contains(flightOptions.DepartureAirportCode)); // Filter by DepartureAirportCode
        }

        if (!string.IsNullOrEmpty(flightOptions.ArrivalAirportCode))
        {
            query = query.Where(f => f.ArrivalAirportCode.Contains(flightOptions.ArrivalAirportCode)); // Filter by ArrivalAirportCode
        }

        if (flightOptions.DepartureDateTime.HasValue) // Not a string
        {
            query = query.Where(f => f.DepartureDateTime.Date <= flightOptions.DepartureDateTime.Value.Date); // Filter by DepartureDateTime
        }
        if (flightOptions.ArrivalDateTime.HasValue) // Not a string
        {
            query = query.Where(f => f.ArrivalDateTime.Date <= flightOptions.ArrivalDateTime.Value.Date); // Filter by DepartureDateTime
        }
        if (flightOptions.SortBy == "id" || !string.IsNullOrEmpty(flightOptions.SortBy))
        // allowing to send other values later on
        {
            switch (flightOptions.SortBy)
            {
                case "airline_name":
                    query = flightOptions.SortOrder == "ASC" ?
                    query.OrderByDescending(f => f.Airline.Name) :
                    query.OrderBy(f => f.Airline.Name);
                    query.OrderByDescending(f => f.Airline.Name);
                    break;
                case "flight_number":
                    query = flightOptions.SortOrder == "ASC" ?
                    query.OrderByDescending(f => f.FlightNumber) :
                    query.OrderBy(f => f.FlightNumber);
                    break;
                case "departure_airport_code":
                    query = flightOptions.SortOrder.ToUpper() == "ASC" ?
                    query.OrderBy(f => f.DepartureAirportCode) :
                    query.OrderByDescending(f => f.DepartureAirportCode);
                    break;
                case "arrival_airport_code":
                    query = flightOptions.SortOrder.ToUpper() == "ASC" ?
                    query.OrderBy(f => f.ArrivalAirportCode) :
                    query.OrderByDescending(f => f.ArrivalAirportCode);
                    break;
                case "departure_date_time":
                    query = flightOptions.SortOrder.ToUpper() == "ASC" ?
                    query.OrderBy(f => f.DepartureDateTime) :
                    query.OrderByDescending(f => f.DepartureDateTime);
                    break;
                case "arrival_date_time":
                    query = flightOptions.SortOrder.ToUpper() == "ASC" ?
                    query.OrderBy(f => f.ArrivalDateTime) :
                    query.OrderByDescending(f => f.ArrivalDateTime);
                    break;
                case "passenger_count":
                    query = flightOptions.SortOrder == "ASC" ?
                    query.OrderBy(f => f.PassengerCapacity) :
                    query.OrderByDescending(f => f.PassengerCapacity);
                    break;
                default:
                    query = flightOptions.SortOrder == "ASC" ?
                    query.OrderBy(f => f.Id) :
                    query.OrderByDescending(f => f.Id);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(flightOptions.Search))
        {
            string searchLower = flightOptions.Search.ToLower();
            var searchChars = searchLower.ToCharArray();

            var data = await query.ToListAsync();

            flights = data.Where(f => searchChars.All(c => f.Airline.Name.ToLower().Contains(c)
            || f.FlightNumber.ToLower().Contains(c)
            || f.Airline.Name.ToLower().Contains(c)
            || f.DepartureAirportCode.ToLower().Contains(c)
            || f.ArrivalAirportCode.ToLower().Contains(c)
            || f.DepartureDateTime.ToString("yyyy-MM-ddTHH:mm:ss").ToLower().Contains(c)
            || f.ArrivalDateTime.ToString("yyyy-MM-ddTHH:mm:ss").ToLower().Contains(c)
            || f.PassengerCapacity.ToString().ToLower().Contains(c)
            )).ToList();
            flights = (List<Flight>)data.Skip((flightOptions.PageNumber - 1) * flightOptions.PageSize)
            .Take(flightOptions.PageSize).ToList();

        }
        else
        {
            query = query.Skip((flightOptions.PageNumber - 1) * flightOptions.PageSize)
            .Take(flightOptions.PageSize); //pagination

            flights = await query.ToListAsync(); // Execute the query and get the results
        }

        bool hasPrevious = flightOptions.PageNumber > 1;
        bool hasNext = (flightOptions.PageNumber * flightOptions.PageSize) < totalFlights;

        return new ApiResponseDto<List<Flight>>
        {
            Data = flights,
            ResponseCode = HttpStatusCode.OK,
            TotalCount = totalFlights,
            CurrentPage = flightOptions.PageNumber,
            PageSize = flightOptions.PageSize,
            HasPreviousPage = hasPrevious,
            HasNextPage = hasNext,
        };
    }

    public async Task<ApiResponseDto<Flight?>> GetFlightById(int id)
    {
        var result = await _dbContext.Flights.Include(f => f.Airline).FirstOrDefaultAsync(f => f.Id == id); // Include the Airline navigation property

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
}
