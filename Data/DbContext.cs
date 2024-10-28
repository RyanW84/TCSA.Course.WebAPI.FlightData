using Microsoft.EntityFrameworkCore;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Data;

public class FlightsDbContext : DbContext
{
    public FlightsDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Flight> Flights { get; set; }
}