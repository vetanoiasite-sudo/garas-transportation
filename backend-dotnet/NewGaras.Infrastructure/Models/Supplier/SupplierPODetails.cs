using System.Runtime.Serialization;

namespace NewGarasAPI.Models.Supplier
{
    public class SupplierPODetails
    {
        private string pO;
        private decimal cost;
        private decimal price;
        private decimal collected;
        private decimal remain;
        private decimal paidPercent;

        [DataMember]
        public string PO { get => pO; set => pO = value; }
        [DataMember]
        public decimal Cost { get => cost; set => cost = value; }
        [DataMember]
        public decimal Price { get => price; set => price = value; }
        [DataMember]
        public decimal Collected { get => collected; set => collected = value; }
        [DataMember]
        public decimal Remain { get => remain; set => remain = value; }
        [DataMember]
        public decimal PaidPercent { get => paidPercent; set => paidPercent = value; }
    }
}
