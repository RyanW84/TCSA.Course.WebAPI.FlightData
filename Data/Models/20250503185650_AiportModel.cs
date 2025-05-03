using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCSA.WebAPI.FlightData.Data.Models
{
    /// <inheritdoc />
    public partial class AiportModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Seats_SeatId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_SeatId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "ArrivalAirportCode",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DepartureAirportCode",
                table: "Flights");

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IataCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirportFlight",
                columns: table => new
                {
                    DepartureAirportsId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightsDepartingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirportFlight", x => new { x.DepartureAirportsId, x.FlightsDepartingId });
                    table.ForeignKey(
                        name: "FK_AirportFlight_Airports_DepartureAirportsId",
                        column: x => x.DepartureAirportsId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirportFlight_Flights_FlightsDepartingId",
                        column: x => x.FlightsDepartingId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirportFlight1",
                columns: table => new
                {
                    ArrivalAirportsId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlightsArrivingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirportFlight1", x => new { x.ArrivalAirportsId, x.FlightsArrivingId });
                    table.ForeignKey(
                        name: "FK_AirportFlight1_Airports_ArrivalAirportsId",
                        column: x => x.ArrivalAirportsId,
                        principalTable: "Airports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirportFlight1_Flights_FlightsArrivingId",
                        column: x => x.FlightsArrivingId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirportFlight_FlightsDepartingId",
                table: "AirportFlight",
                column: "FlightsDepartingId");

            migrationBuilder.CreateIndex(
                name: "IX_AirportFlight1_FlightsArrivingId",
                table: "AirportFlight1",
                column: "FlightsArrivingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirportFlight");

            migrationBuilder.DropTable(
                name: "AirportFlight1");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Seats",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArrivalAirportCode",
                table: "Flights",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartureAirportCode",
                table: "Flights",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatId",
                table: "Seats",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Seats_SeatId",
                table: "Seats",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id");
        }
    }
}
