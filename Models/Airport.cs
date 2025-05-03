using System.ComponentModel.DataAnnotations;

namespace TCSA.WebAPI.FlightData.Models;

public class Airport
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IataCode { get; set; } = string.Empty;
    public virtual ICollection<Flight> FlightsDeparting { get; set; } = [];
    public virtual ICollection<Flight> FlightsArriving { get; set; } = [];
}
