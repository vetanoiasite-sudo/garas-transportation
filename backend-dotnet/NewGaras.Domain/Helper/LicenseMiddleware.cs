
using NewGaras.Infrastructure.Models.TransportationLineModel;

namespace NewGaras.Domain.Helper
{
    public class LicenseMiddleware
    {
        private readonly RequestDelegate _next;

        public LicenseMiddleware(RequestDelegate next , IConfiguration config)
        {
            _next=next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            // 1. قراءة القيم من appsettings.json
            var authorizedmotherboard = "E9AD6AEF024D99188E10765EEBAB6E96FA2B9A7FC0CA55651375FFAC6755FFD1";
            var authorizedUuid = "0EBC5C05A714034DE42FD2EB44F7E8CBC03B17550B5E33AE4AC3EB57C3E04503";
            DateTime encodedExpiry = new DateTime(2027, 01, 01);

            // 2. حساب هاش الجهاز الحالي (مرة واحدة فقط عند أول طلب لسرعة الأداء)
          
                var deviceIdentifier = DeviceIdentifier.GetUniqueId();
          

            // 3. التحقق من تطابق الجهاز
            if(deviceIdentifier.motherboardId != authorizedmotherboard || deviceIdentifier.uuid != authorizedUuid)
            {
                context.Response.StatusCode=403;
                await context.Response.WriteAsJsonAsync(new { message = "Unauthorized Hardware ID." });
                return;
            }

         
            if(DateTime.Now > encodedExpiry)
            {
                context.Response.StatusCode=403;
                await context.Response.WriteAsJsonAsync(new { message = "License has expired." });
                return;
            }

            // إذا كان كل شيء سليم، مرر الطلب للـ Controller المطلوب
            await _next(context);
        }



    }
}
