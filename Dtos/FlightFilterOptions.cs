using Microsoft.AspNetCore.Mvc;

namespace TCSA.WebAPI.FlightData.Dtos;

public class FlightFilterOptions
{
    [FromQuery(Name = "airline_name")]
    public string AirlineName { get; set; }=string.Empty;
    [FromQuery(Name = "departure_airport_code")]
    public string DepartureAirportCode { get; set; } = string.Empty;
    [FromQuery(Name = "arrival_airport_code")]
    public string ArrivalAirportCode { get; set; } = string.Empty;
    [FromQuery(Name = "departure_date_time")]
    public DateTime? DepartureDateTime { get; set; }
    [FromQuery(Name = "arrival_date_time")]
    public DateTime? ArrivalDateTime { get; set; }
}
