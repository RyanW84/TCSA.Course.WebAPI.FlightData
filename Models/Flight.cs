using System.ComponentModel.DataAnnotations;

namespace TCSA.WebAPI.FlightData.Models;

public class Flight
{
    [Key]
    public int Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string DepartureAirportCode { get; set; } = string.Empty;
    public string ArrivalAirportCode { get; set; } = string.Empty;
    public DateTime DepartureDateTime { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public int PassengerCapacity { get; set; }

    public virtual Airline? Airline { get; set; } = null!; // Using virtual to enable lazy loading and change tracking for related entities
    public virtual ICollection<Seat> Seats { get; set; } = []; // Using ICollection to allow for multiple seats per flight
}
