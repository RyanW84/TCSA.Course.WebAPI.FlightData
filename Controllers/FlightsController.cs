using Microsoft.AspNetCore.Mvc;
using TCSA.WebAPI.FlightData.Models;
using TCSA.WebAPI.FlightData.Services;

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
    public async Task <ActionResult<Flight>> CreateFlight(Flight flight)
    {
        var createdFlight = await _flightService.Createflight(flight);

        return new ObjectResult(createdFlight) { StatusCode = 201 }; //05:36
     
    }

    [HttpPut]
    public ActionResult<Flight> UpdateFlight(Flight flight)
    {
        var result = _flightService.GetFlightById(flight.Id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public ActionResult<Flight> DeleteFlight(int id)
    {
        var result = _flightService.GetFlightById(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
