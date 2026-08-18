namespace NewGaras.Infrastructure.DTO.VacationType
{
    public class AbsenceHistory
    {
        public long Id { get; set; }
        public long HrUserId {  get; set; }
        public string HrUserName { get; set; }

        public string AbsenceName { get; set; }

        public DateOnly Date {  get; set; }

        public string AbsenceCause { get; set; }

        public long ApprovedAbsenceById { get; set; }

        public string ApprovedAbsenceName { get; set; }

        public string ApprovedCause { get; set; }

        public DateOnly ApprovedDate { get; set; }
    }
}