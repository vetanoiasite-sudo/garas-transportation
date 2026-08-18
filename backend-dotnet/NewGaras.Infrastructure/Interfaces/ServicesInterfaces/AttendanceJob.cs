
using Microsoft.Extensions.DependencyInjection;
using NewGaras.Infrastructure.Helper.TenantService;
using NewGaras.Infrastructure.Interfaces.ServicesInterfaces.TransportationLine;
using Quartz;
namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces
{
    public class AttendanceJob : IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AttendanceJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory=scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();

                // 1. تفعيل الـ Tenant يدوياً قبل طلب أي خدمة أخرى
                // هذا سيجعل الـ Constructor الخاص بالـ DbContext يعمل بسلام
                tenantService.SetTenant("pharma");

                // 2. جلب الخدمة مباشرة (الآن الـ Context داخلها سيعرف الـ Provider من OnConfiguring)
                var attendanceService = scope.ServiceProvider.GetRequiredService<ITransportationLineService>();

                try
                {
                    attendanceService.AddUsersAttedance66();
                    Console.WriteLine("Attendance Job Finished Successfully.");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Critical Error: {ex.Message}");
                    if(ex.InnerException!=null)
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
