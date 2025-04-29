using Microsoft.AspNetCore.Mvc;
using TCSA.WebAPI.FlightData.Models;
using TCSA.WebAPI.FlightData.Services;
using TCSA.WebAPI.FlightData.Dtos;
using AutoMapper;

namespace TCSA.WebAPI.FlightData.Controllers;
[ApiController]
[Route("api/[controller]")]
//Example: http:localhost:5609/api/flights
public class FlightsController(IFlightService flightService) : ControllerBase
{
    private readonly IFlightService _flightService = flightService;
  

    [HttpGet]
    public async Task<ActionResult<List<Flight>>> GetAllFlights()
    {
        var flights = await _flightService.GetAllFlights();

        return Ok(flights);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Flight>> GetFlightById(int id)
    {
        var result = await _flightService.GetFlightById(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Flight>> CreateFlight(FlightApiRequestDto flight)
    {
    
        var createdFlight = await _flightService.CreateFlight(flight);

        return new ObjectResult(createdFlight) { StatusCode = 201 };
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Flight>> UpdateFlight(int id, FlightApiRequestDto updatedFlight)
    {
        var result = await _flightService.UpdateFlight(id, updatedFlight);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>>DeleteFlight(int id)
    {
        var result = await _flightService.DeleteFlight(id);

        if (result == null)
        {
            return NotFound();
        }

        return new ObjectResult(result) {StatusCode = 204 };
    }
}
