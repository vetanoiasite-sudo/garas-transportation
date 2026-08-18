
namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class SuppliersAccountDto
    {
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalPrice { get; set; }
        public List<RoundDto> DetailsRounds { get; set; }
    }

    public class RoundDto
    {
        public string LineName { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalRounds { get; set; }
    }
    public class priceRoundDto
    {
        public decimal TotalPrice { get; set; }
        public int TotalRounds { get; set; }
    }
}
