namespace NewGarasAPI.Models.HR
{
    public class ContractLeaveEmployeeModel
    {
        public long? Id { set; get; }
        public int ContractLeaveSettingID { set; get; }
        public string LeaveAllowed { set; get; }
        public int Balance { set; get; }
        public int Used { set; get; }
        public int Remain {  set; get; }

        public long HrUserId { set; get; }
    }
}
