using System.ComponentModel.DataAnnotations;

namespace TCSA.WebAPI.FlightData.Models;

public class Seat
{
    [Key]
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int FlightID { get; set; }
    public virtual Flight? Flight { get; set; }
}
