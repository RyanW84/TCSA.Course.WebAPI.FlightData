using Microsoft.EntityFrameworkCore;
using TCSA.WebAPI.FlightData.Models;

namespace TCSA.WebAPI.FlightData.Data;

public class FlightsDbContext : DbContext
{
    public FlightsDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<Airline> Airlines { get; set; }
    public DbSet<Seat> Seats { get; set; } // Add DbSet for Seat entity
    public DbSet<Airport> Airports { get; set; } // Add DbSet for Airport entity

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Flight>()
            .HasOne(f => f.Airline) // has an Airline property
            .WithOne(a => a.Flight) // has a Flight property
            .HasForeignKey<Airline>(A => A.FlightID) // foreign key in Airline table
            .OnDelete(DeleteBehavior.Cascade); // delete behavior

        modelBuilder
            .Entity<Flight>()
            .HasMany(f => f.DepartureAirports) // has a collection of DepartureFlights
            .WithMany(a => a.FlightsDeparting); // has a collection of Flights

        modelBuilder
            .Entity<Flight>()
            .HasMany(f => f.ArrivalAirports) // has a collection of ArrivalFlights
            .WithMany(a => a.FlightsArriving); // has a collection of Flights

        modelBuilder
            .Entity<Seat>()
            .HasOne(s => s.Flight) // has a Flight property
            .WithMany(f => f.Seats) // has a collection of Seats
            .HasForeignKey(s => s.FlightID) // foreign key in Seat table
            .OnDelete(DeleteBehavior.Cascade); // delete behavior
    }

    public void SeedData()
    {
        Airports.RemoveRange(Airports);

        Airport lax = new Airport { Name = "Los Angeles International Airport", IataCode = "LAX" };
        Airport jfk = new Airport
        {
            Name = "John F. Kennedy International Airport",
            IataCode = "JFK",
        };
        Airport par = new Airport { Name = "Paris Charles de Gaulle Airport", IataCode = "CDG" };
        Airport tky = new Airport { Name = "Tokyo Haneda Airport", IataCode = "HND" };
        Airport fll = new Airport
        {
            Name = "Fort Lauderdale-Hollywood International Airport",
            IataCode = "FLL",
        };

        var airports = new List<Airport> { lax, jfk, par, tky, fll };
        Airports.AddRange(airports); // add new data to Airports

        Airlines.RemoveRange(Airlines); // remove data from Airlines table to ensure consistency for
        var airlines = new List<Airline>
        {
            new Airline { Name = "American Airlines" },
            new Airline { Name = "United Airlines" },
            new Airline { Name = "Delta Airlines" },
        };

        Airlines.AddRange(airlines); // add new data to Airlines
        Seats.RemoveRange(Seats); // remove data from Seats table to ensure consistency for

        var seatsA = new List<Seat>
        {
            new Seat { SeatNumber = "A1" },
            new Seat { SeatNumber = "A2" },
            new Seat { SeatNumber = "A3" },
            new Seat { SeatNumber = "A4" },
            new Seat { SeatNumber = "A5" },
            new Seat { SeatNumber = "A6" },
        };
        var seatsB = new List<Seat>
        {
            new Seat { SeatNumber = "B1" },
            new Seat { SeatNumber = "B2" },
            new Seat { SeatNumber = "B3" },
            new Seat { SeatNumber = "B4" },
            new Seat { SeatNumber = "B5" },
            new Seat { SeatNumber = "B6" },
        };
        var seatsC = new List<Seat>
        {
            new Seat { SeatNumber = "C1" },
            new Seat { SeatNumber = "C2" },
            new Seat { SeatNumber = "C3" },
            new Seat { SeatNumber = "C4" },
            new Seat { SeatNumber = "C5" },
            new Seat { SeatNumber = "C6" },
        };

        Seats.AddRange(seatsA);
        Seats.AddRange(seatsB);
        Seats.AddRange(seatsC);

        Flights.RemoveRange(Flights);

        Flights.AddRange(
            new Flight
            {
                Id = 1,
                FlightNumber = "AA-101",
                DepartureDateTime = DateTime.Now.AddHours(2),
                ArrivalDateTime = DateTime.Now.AddHours(5),
                PassengerCapacity = 140,
                Airline = airlines[0], // set the Airline property
                Seats = seatsA, // set the Seats property
                DepartureAirports = [lax],
                ArrivalAirports = [jfk],
            },
            new Flight
            {
                Id = 2,
                FlightNumber = "AB-202",
                DepartureDateTime = DateTime.Now.AddHours(3),
                ArrivalDateTime = DateTime.Now.AddHours(6),
                PassengerCapacity = 100,
                Airline = airlines[1],
                Seats = seatsB, // set the Seats property
                DepartureAirports = [par],
                ArrivalAirports = [tky],
            },
            new Flight
            {
                Id = 3,
                FlightNumber = "AC-303",
                DepartureDateTime = DateTime.Now.AddHours(3),
                ArrivalDateTime = DateTime.Now.AddHours(6),
                PassengerCapacity = 120,
                Airline = airlines[2],
                Seats = seatsC, // set the Seats property
                DepartureAirports = [jfk],
                ArrivalAirports = [fll],
            }
        );

        SaveChanges();
    }
}
