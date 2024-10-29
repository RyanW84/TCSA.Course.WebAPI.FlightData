using Microsoft.AspNetCore.Mvc;
using TCSA.WebAPI.FlightData.Models;
using TCSA.WebAPI.FlightData.Services;

namespace TCSA.WebAPI.FlightData.Controllers;

[Route("api/[controller]")]
//Example: http:localhost:5609/api/flights
public class FlightsController(IFlightService flightService) : Controller
{
    private readonly IFlightService _flightService = flightService;

    [HttpGet]
    public ActionResult<List<Flight>> GetAllFlights()
    {
        return Ok(_flightService.GetFlights());
    }

    [HttpGet("{id}")]
    public ActionResult<Flight> GetFlightById(int id)
    {
        var result = _flightService.GetFlightById(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<Flight> CreateFlight(Flight flight)
    {
        return Ok(_flightService.Createflight(flight));
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

    [HttpPut("{id}")]
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
