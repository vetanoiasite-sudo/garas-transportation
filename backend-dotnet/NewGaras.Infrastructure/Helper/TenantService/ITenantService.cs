using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Helper.TenantService
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();

        public string GetConnectionString();

        public Tenant GetTenant();

        public bool SetTenant(string tenantId);

        //ApplicationDbContext CreateDbContext(ITenantService _tenantService);
    }
}
