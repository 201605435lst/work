using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace SnAbp.TenantManagement
{
    public class Tenant_Tests : SnAbpTenantManagementDomainTestBase
    {
        private readonly ITenantRepository _tenantRepository;

        public Tenant_Tests()
        {
            _tenantRepository = GetRequiredService<ITenantRepository>();
        }

        [Fact]
        public async Task FindDefaultConnectionString()
        {
            var acme = await _tenantRepository.FindByNameAsync("acme");

            acme.ShouldNotBeNull();
            acme.FindDefaultConnectionString().ShouldBe("DefaultConnString-Value");
        }

        [Fact]
        public async Task FindConnectionString()
        {
            var acme = await _tenantRepository.FindByNameAsync("acme");

            acme.ShouldNotBeNull();
            acme.FindConnectionString(Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName).ShouldBe("DefaultConnString-Value");
            acme.FindConnectionString("MyConnString").ShouldBe("MyConnString-Value");
        }
    }
}
