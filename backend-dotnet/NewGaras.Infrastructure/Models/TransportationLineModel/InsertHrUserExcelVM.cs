

using System.ComponentModel.DataAnnotations;

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class InsertHrUserExcelVM
    {
        [Required]
        public IFormFile ExcelSheet { get; set; }

    }
}
