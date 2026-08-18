
namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class AccountsAllRoundsDto
    {
        public string NameOfRoute { get; set; }
        public string Serial { get; set; }
        public string? DateOfRound { get; set; }
        public DateTime? DateOfCheckIn { get; set; }
        public DateTime? DateOfCheckOut { get; set; }
        public DateTime? DateOfOneWay { get; set; }
        public double RoundsNum { get; set; }
        public decimal TotalPriceOfDay { get; set; }

    }
}
