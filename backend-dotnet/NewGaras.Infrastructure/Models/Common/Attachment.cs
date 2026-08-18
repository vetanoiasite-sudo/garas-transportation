namespace NewGarasAPI.Models.Common
{
    public class Attachment
    {
        public long? Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileContent { get; set; }
        public string FilePath { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
    }
}
