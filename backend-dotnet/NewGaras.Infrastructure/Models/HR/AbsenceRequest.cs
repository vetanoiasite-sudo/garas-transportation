namespace NewGaras.Infrastructure.Models.HR
{
    public class AbsenceRequest
    {
        public string From {  get; set; }
        public string To {  get; set; }
        public string Approval {  get; set; }
        public string AbsenceCause {  get; set; }
        public bool? FirstApproval { get; set; }
        public string FirstRejectionCause { get; set; }
        public string FirstApprovedByName { get; set; }
        public string FirstApprovedByImg { get; set; }
        public bool? SecondApproval { get; set; }
        public string SecondRejectionCause { get; set; }
        public string SecondApprovedByName { get; set; }
        public string SecondApprovedByImg { get; set; }
    }
}