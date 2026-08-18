namespace NewGarasAPI.Models.HR
{
    public class JobTitleData
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
    }
}
