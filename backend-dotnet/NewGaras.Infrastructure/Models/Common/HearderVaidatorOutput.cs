namespace NewGarasAPI.Models.Common
{
    public class HearderVaidatorOutput
    {
        public bool result;
        public List<Error> errors;

        public long userID;
        public string CompanyName;
        public string UserToken { get; set; }
    }
}
