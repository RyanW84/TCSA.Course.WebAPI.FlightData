using Microsoft.EntityFrameworkCore;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Data;

public class FlightsDbContext : DbContext
{
    public FlightsDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Flight> Flights { get; set; }

    public void SeedData()
    {
        Flights.RemoveRange(Flights);

        Flights.AddRange(
            new Flight
            {
                Id = 1,
                FlightNumber = 123,
                AirlineName = "Airline A",
                DepartureAirportCode = "JFK",
                ArrivalAirportCode = "LAX",
                DepartureDateTime = DateTime.Now.AddHours(2),
                ArrivalDateTime = DateTime.Now.AddHours(5),
                PassengerCapacity = 150
            },
            new Flight
            {
                Id = 2,
                FlightNumber = 456,
                AirlineName = "Airline B",
                DepartureAirportCode = "ORD",
                ArrivalAirportCode = "DFW",
                DepartureDateTime = DateTime.Now.AddHours(3),
                ArrivalDateTime = DateTime.Now.AddHours(6),
                PassengerCapacity = 200
            }
        );

        SaveChanges();
    }
}