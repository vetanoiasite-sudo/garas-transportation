
namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class AccountDto
    {
        public string MonthName { get; set; }
        public int MonthNum { get; set; }
        public long AccountId { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        //public int Month { get; set; }
        public int RoutesNum { get; set; }
       // public int CountOfRounds { get; set; }
        public double CountOfcompleteRounds { get; set; }
        public int CountOfHalfGoRounds { get; set; }
        public int CountOfHalfReturnRounds { get; set; }
        public decimal? TotalDue { get; set; }
        public decimal? TotalDeduct { get; set; }
        public decimal? TotalTaxesDeduct { get; set; }
        public decimal? TotalNormalDeduct { get; set; }
        public decimal? TotalPaidadvance { get; set; }
        public decimal? TotalPaidNormal { get; set; }
        public decimal? TotalDueAfterPaid { get; set; }
        public string? Note { get; set; }


        public decimal? TotalDuecompleteRound { get; set; }
        public decimal? TotalDueHalfGoRound { get; set; }
        public decimal? TotalDueHalfReturnRound { get; set; }

    }

}
