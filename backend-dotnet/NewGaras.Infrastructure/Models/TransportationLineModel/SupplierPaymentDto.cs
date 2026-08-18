

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class SupplierPaymentDto
    {
        public string SupplierName { get; set; }

        public decimal Payment { get; set; }

        public DateTime DatePayment { get; set; }

        public DateTime? StartDate { get; set; }

        public int? NumberOfMonths { get; set; }

        public string TypeOfDebt { get; set; }
        public List<DistributionSupplierPaymentDo> DistributionSupplierPayments { get; set; } = new List<DistributionSupplierPaymentDo>();
    }
}
