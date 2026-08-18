namespace NewGarasAPI.Models.HR
{
    public class AbsenceDetailsViewModel
    {
        public int ContractLeaveSettingID { get; set; }
        public string HolidayName { get; set; }
        public string leaveAllowed {  get; set; }
        public int balance { get; set; }
        public int used {  get; set; }
        public int remain { get; set; }
    }
}
