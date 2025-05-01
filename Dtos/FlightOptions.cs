using Microsoft.AspNetCore.Mvc;

namespace TCSA.WebAPI.FlightData.Dtos;

public class FlightOptions
    {

    // Filter options
    [FromQuery(Name = "airline_name")]
    public string AirlineName { get; set; } = string.Empty;
    [FromQuery(Name = "departure_airport_code")]
    public string DepartureAirportCode { get; set; } = string.Empty;
    [FromQuery(Name = "arrival_airport_code")]
    public string ArrivalAirportCode { get; set; } = string.Empty;
    [FromQuery(Name = "departure_date_time")]
    public DateTime? DepartureDateTime { get; set; }
    [FromQuery(Name = "arrival_date_time")]
    public DateTime? ArrivalDateTime { get; set; }

    // Sorting options
    [FromQuery(Name = "sort_by")]
    public string SortBy { get; set; } = "id";
    [FromQuery(Name = "sort_order")]
    public string SortOrder { get; set; } = "ASC"; // Asc or Desc

    // Search options
    [FromQuery(Name = "search")]
    public string Search { get; set; } = string.Empty;

    // Pagination options
    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 10; // Default page size
    [FromQuery(Name = "page_number")]
    public int PageNumber { get; set; } = 1; // Default page number
    }
