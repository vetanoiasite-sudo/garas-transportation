namespace NewGaras.Infrastructure.Models.Inventory
{
    public class AttachmentFile
    {
        public long? Id { get; set; }
        public bool Active { get; set; }

        public IFormFile File { get; set; }

        public string FilePath { get; set; }
    }
}