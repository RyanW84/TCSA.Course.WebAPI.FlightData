using Microsoft.AspNetCore.Mvc;

using TCSA.WebAPI.FlightData.Dtos;
using TCSA.WebAPI.FlightData.Models;
using TCSA.WebAPI.FlightData.Services;

namespace TCSA.WebAPI.FlightData.Controllers;
[ApiController]
[Route("api/[controller]")]
//Example: http:localhost:5609/api/flights 
public class FlightController(IFlightService flightService): ControllerBase
    {
    private readonly IFlightService _flightService = flightService;


    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<List<Flight>>>> GetAllFlights(FlightFilterOptions filterOptions)
        {
        var flights = await _flightService.GetAllFlights(filterOptions);

        return Ok(await _flightService.GetAllFlights(filterOptions));
        }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponseDto<Flight>>> GetFlightById(int id)
        {
        var result = await _flightService.GetFlightById(id);

        if(result == null)
            {
            return NotFound();
            }

        return Ok(result);
        }

    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<Flight>>> CreateFlight(FlightApiRequestDto flight)
        {
        if(!ModelState.IsValid)
            {
            return BadRequest(ModelState);
            }

        var createdFlight = await _flightService.CreateFlight(flight);

        return new ObjectResult(createdFlight) { StatusCode = 201 };
        }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponseDto<Flight>>> UpdateFlight(int id , FlightApiRequestDto updatedFlight)
        {

        if(!ModelState.IsValid)
            {
            return BadRequest(ModelState);
            }

        var result = await _flightService.UpdateFlight(id , updatedFlight);

        if(result == null)
            {
            return NotFound();
            }

        return Ok(result);
        }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponseDto<string>>> DeleteFlight(int id)
        {
        var result = await _flightService.DeleteFlight(id);

        if(result == null)
            {
            return NotFound();
            }

        return NoContent();
        }
    }
