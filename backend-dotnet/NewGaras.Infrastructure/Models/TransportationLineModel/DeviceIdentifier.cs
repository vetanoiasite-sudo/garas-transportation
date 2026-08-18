using System.Management;
using System.Runtime.Versioning;

namespace NewGaras.Infrastructure.Models.TransportationLineModel
{
    public class DeviceIdentifier
    {
        [SupportedOSPlatform("windows")]
        public static DeviceIdentifierDto GetUniqueId()
        {
            string cpuId = "N/A";
            string motherboardId = "N/A";
            string uuid = "N/A";
            string diskSerial = "N/A";

            try
            {
                // 1. جلب رقم المعالج (CPU ID)
                //using(var searcher = new ManagementObjectSearcher("Select ProcessorId From Win32_Processor"))
                //{
                //    foreach(var obj in searcher.Get())
                //        cpuId=obj ["ProcessorId"]?.ToString()??"N/A";
                //}

                // 2. جلب رقم اللوحة الأم (Motherboard ID)
                using(var searcher = new ManagementObjectSearcher("Select SerialNumber From Win32_BaseBoard"))
                {
                    foreach(var obj in searcher.Get())
                        motherboardId=obj ["SerialNumber"]?.ToString()??"N/A";
                }

                // 3. جلب الـ UUID (فريد جداً للسيرفرات)
                using(var searcher = new ManagementObjectSearcher("Select UUID From Win32_ComputerSystemProduct"))
                {
                    foreach(var obj in searcher.Get())
                        uuid=obj ["UUID"]?.ToString()??"N/A";
                }

                // 4. جلب الرقم التسلسلي للقرص الصلب (Disk Serial)
                //using(var searcher = new ManagementObjectSearcher("Select SerialNumber From Win32_PhysicalMedia"))
                //{
                //    foreach(var obj in searcher.Get())
                //    {
                //        diskSerial=obj ["SerialNumber"]?.ToString()??"N/A";
                //        break; // نأخذ القرص الأول فقط
                //    }
                //}
            }
            catch
            {
                // في حال حدوث خطأ في الوصول لبيانات WMI
            }

            // تنظيف النصوص من المسافات الزائدة لضمان مطابقة الـ Hash لاحقاً
           // cpuId=cpuId.Replace(" " , "").Trim();
           motherboardId=motherboardId.Replace(" " , "").Trim();
            uuid=uuid.Replace(" " , "").Trim();
          //  diskSerial=diskSerial.Replace(" " , "").Trim();

            using var sha256motherboard= System.Security.Cryptography.SHA256.Create();
            var hashBytesmotherboard= sha256motherboard.ComputeHash(System.Text.Encoding.UTF8.GetBytes(motherboardId));
            string hashedmotherboardId= Convert.ToHexString(hashBytesmotherboard);

            using var sha256uuid = System.Security.Cryptography.SHA256.Create();
            var hashBytesuuid = sha256uuid.ComputeHash(System.Text.Encoding.UTF8.GetBytes(uuid));
            string hasheduuid= Convert.ToHexString(hashBytesuuid);



            //string fullId = "6897-5348-8523-2258-5362-2768-31";
            //using var sha256 = System.Security.Cryptography.SHA256.Create();
            //var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fullId));
            //string hashedKey = Convert.ToHexString(hashBytes);

            //string fullId22 = "C160148B-6497-499F-AB5C-E9CBB3D51DD3";
            //using var sha256_22 = System.Security.Cryptography.SHA256.Create();
            //var hashBytes22 = sha256_22.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fullId22));
            //string hashedKey22 = Convert.ToHexString(hashBytes22);


            var DeviceIdentifierDto = new DeviceIdentifierDto
            {
                motherboardId = hashedmotherboardId,
                uuid = hasheduuid
            };

            // إرجاع الـ FullID بنفس التنسيق الذي ظهر لك في الـ JSON
            return DeviceIdentifierDto;
        }
    }
}
