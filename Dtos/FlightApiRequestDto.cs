using System.ComponentModel.DataAnnotations;

namespace TCSA.WebAPI.FlightData.Dtos
{
    public class FlightApiRequestDto
    {
        [Required]
        [MaxLength(8, ErrorMessage = "Flight Number must have 8 Characters or less!")]
        [MinLength(6, ErrorMessage = "Flight Number must have 6 Characters or more!")]
        public string FlightNumber { get; set; } = string.Empty;

        [Required]
        public string AirlineName { get; set; }

        [Required]
        public DateTime DepartureDateTime { get; set; }

        [Required]
        public DateTime ArrivalDateTime { get; set; }

        [Required]
        [Range(90, 140)]
        public int PassengerCapacity { get; set; }
    }
}
