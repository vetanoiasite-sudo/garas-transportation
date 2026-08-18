namespace NewGarasAPI.Models.HR
{
    public class UserAttendance
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<TeamModel> TeamList { get; set; }
        public long AttendanceId { get; set; }
        public int? AbsenceTypeId { get; set; }
        public string AbsenceTypeName { get; set; }
        public int? CheckInHour { get; set; }
        public int? CheckInMin {  get; set; }
        public int? CheckOutHour { get; set; }
        public int? CheckOutMin { get; set; }
        public bool? IsApprovedAbsence { get; set; }
        public bool? IsOffDay { get; set; }
        public string OffDayName { get; set; }
    }
}
