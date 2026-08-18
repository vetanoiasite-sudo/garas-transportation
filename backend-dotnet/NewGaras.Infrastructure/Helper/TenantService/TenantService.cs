using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace NewGaras.Infrastructure.Helper.TenantService
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private readonly IConfiguration _configuration;
        private HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(
            IOptions<TenantSettings> tenantSettings ,
            IHttpContextAccessor contextAccessor ,
            IConfiguration configuration)
        {
            _tenantSettings=tenantSettings.Value;
            _configuration=configuration;
            _httpContext=contextAccessor.HttpContext;

            if(_httpContext!=null)
            {
                if(_httpContext.Request.Headers.TryGetValue("CompanyName" , out var tenantId))
                {
                    SetTenant(tenantId!);
                }
                else
                {
                    SetTenant("pharma"); // Use lowercase to match TID
                }
            }
        }

        public bool SetTenant(string tenantId)
        {
            _currentTenant=_tenantSettings.Tenants
                .FirstOrDefault(a => a.TID.ToLower()==tenantId.ToLower());

            if(_currentTenant==null)
                return false;

            // Set connection string from TenantSettings first, 
            // then fallback to appsettings.json ConnectionStrings, 
            // then use default
            if(string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }

            return true;
        }

        private void SetDefaultConnectionStringToCurrentTenant()
        {
            // First, try to get from appsettings.json ConnectionStrings section
            var connectionStringFromConfig = _configuration
                .GetConnectionString(_currentTenant.TID);

            if(!string.IsNullOrEmpty(connectionStringFromConfig))
            {
                _currentTenant.ConnectionString=connectionStringFromConfig;
            }
            else
            {
                // Fallback to TenantSettings default
                _currentTenant.ConnectionString=_tenantSettings.Defaults.ConnectionString;
            }
        }

        public string GetConnectionString()
        {
            if(_currentTenant?.TID!=null)
            {
                var connectionString = _tenantSettings.Tenants
                    .FirstOrDefault(a => a.TID.ToLower() == _currentTenant.TID.ToLower())
                    ?.ConnectionString;

                return connectionString??_tenantSettings.Defaults.ConnectionString;
            }

            return _currentTenant?.ConnectionString??_tenantSettings.Defaults.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            return _tenantSettings.Defaults?.DBProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}
