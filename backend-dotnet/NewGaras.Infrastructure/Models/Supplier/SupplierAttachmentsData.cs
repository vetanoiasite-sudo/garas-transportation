using NewGaras.Infrastructure.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.Supplier
{
    public class SupplierAttachmentsData
    {
        public long SupplierId { get; set; }
        public AttachmentFile TaxCardAttachment { get; set; }
        public AttachmentFile CommercialRecordAttachment { get; set; }
        public List<AttachmentFile> LicenseAttachements { get; set; }
        public List<AttachmentFile> BussinessCardsAttachments { get; set; }
        public List<AttachmentFile> BrochuresAttachments { get; set; }
        public List<AttachmentFile> OtherAttachments { get; set; }
    }
}
