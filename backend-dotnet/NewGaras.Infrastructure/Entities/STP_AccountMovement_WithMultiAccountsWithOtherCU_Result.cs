using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Entities
{
    public class STP_AccountMovement_WithMultiAccountsWithOtherCU_Result
    {
        public long ID { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountCategoryName { get; set; }
        public string AccountTypeName { get; set; }
        public string CurrencyName { get; set; }
        public long aofjeID { get; set; }
        public long EntryID { get; set; }
        public string Serial { get; set; }
        public Nullable<long> MethodID { get; set; }
        public string SignOfAccount { get; set; }
        public string MethodName { get; set; }
        public string DailyCreatedFirstName { get; set; }
        public string DailyCreatedLastName { get; set; }
        public string FromOrTo { get; set; }
        public string DocumentNumber { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public Nullable<bool> PrivateJE { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string EntryAccountComment { get; set; }
        public System.DateTime EntryDate { get; set; }
        public Nullable<decimal> RateToLocalCU { get; set; }
        public Nullable<decimal> CreditOtherCU { get; set; }
        public Nullable<decimal> DebitOtherCU { get; set; }
        public Nullable<decimal> AmountOtherCU { get; set; }
        public Nullable<decimal> ACCOtherCU { get; set; }
        public Nullable<decimal> Acc_Calc { get; set; }
    }

}
