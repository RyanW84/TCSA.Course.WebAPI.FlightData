using Microsoft.EntityFrameworkCore;

using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Data;

public class FlightsDbContext: DbContext
    {
    public FlightsDbContext(DbContextOptions options) : base(options)
        {

        }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<Airline> Airlines { get; set; } // setting Tables

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder.Entity<Flight>()
            .HasOne(f => f.Airline) // has an Airline property
            .WithOne(a => a.Flight) // has a Flight property
            .HasForeignKey<Airline>(A => A.FlightID) // foreign key in Airline table
            .OnDelete(DeleteBehavior.Cascade); // delete behavior
        }

    public void SeedData()
        {
        Airlines.RemoveRange(Airlines); // remove data from Airlines table to ensure consistency for testing

        var airlines = new List<Airline>
            {
            new Airline
                {
                Name = "American Airlines"
                },
            new Airline
                {
                Name = "United Airlines"
                },
            new Airline
                {
                Name = "Delta Airlines"
                }
            };

        Airlines.AddRange(airlines); // add new data to Airlines table

        Flights.RemoveRange(Flights);

        Flights.AddRange(
            new Flight
                {
                Id = 1,
                FlightNumber = "AA-101",
                DepartureAirportCode = "JFK",
                ArrivalAirportCode = "LAX",
                DepartureDateTime = DateTime.Now.AddHours(2),
                ArrivalDateTime = DateTime.Now.AddHours(5),
                PassengerCapacity = 140,
                Airline = airlines[0] // set the Airline property
                },
            new Flight
                {
                Id = 2,
                FlightNumber = "AB-202",
                DepartureAirportCode = "ORD",
                ArrivalAirportCode = "DFW",
                DepartureDateTime = DateTime.Now.AddHours(3),
                ArrivalDateTime = DateTime.Now.AddHours(6),
                PassengerCapacity = 100,
                Airline = airlines[1]
                },
            new Flight
                {
                Id = 3,
                FlightNumber = "AC-303",
                DepartureAirportCode = "ORD",
                ArrivalAirportCode = "DFW",
                DepartureDateTime = DateTime.Now.AddHours(3),
                ArrivalDateTime = DateTime.Now.AddHours(6),
                PassengerCapacity = 120,
                Airline = airlines[2]
                }
        );

        SaveChanges();
        }
    }