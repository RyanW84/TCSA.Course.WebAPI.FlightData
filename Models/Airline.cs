using System.ComponentModel.DataAnnotations;

namespace TCSA.WebAPI.FlightData.Models;

public class Airline
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int FlightID { get; set; }
    public virtual Flight? Flight { get; set; } = null!; // Using virtual to enable lazy loading and change tracking for related entities
}
