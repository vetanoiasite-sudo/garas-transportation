using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class ContractLeaveSettingRequest
    {
        long? iD;
        string holidayName;
        decimal balancePerYear;
        decimal balancePerMonth;
        string note;

        [DataMember]
        public long? ID
        {
            set { iD = value; }
            get { return iD; }
        }
        [DataMember]
        public string HolidayName
        {
            set { holidayName = value; }
            get { return holidayName; }
        }
        [DataMember]
        public decimal BalancePerYear
        {
            set { balancePerYear = value; }
            get { return balancePerYear; }
        }
        [DataMember]
        public decimal BalancePerMonth
        {
            set { balancePerMonth = value; }
            get { return balancePerMonth; }
        }
        [DataMember]
        public string Note
        {
            set { note = value; }
            get { return note; }
        }
    }
}
