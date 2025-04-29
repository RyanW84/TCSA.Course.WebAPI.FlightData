
namespace TCSA.WebAPI.FlightData.Dtos
{
    public class FlightApiRequestDto
    {
        public int FlightNumber { get; set; }
        public string AirlineName { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public int PassengerCapacity { get; set; }
    }
}