

namespace NewGaras.Infrastructure.DTO
{
    public class ReservationInvoiceDto
    {
        public int Id { get; set; }

        public string InvoiceDate { get; set; }

        public decimal Amount { get; set; }

        public string CreateDate { get; set; }

        public string? CreateBy { get; set; }

        
        public string? Serial { get; set; }


        public string ClientName { get; set; }


        public string InvoiceTypeName { get; set; }

        public string CurrencyName { get; set; }
    }
}
