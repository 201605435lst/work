using Microsoft.Extensions.Localization;
using Shouldly;
using SnAbp.TenantManagement.Localization;
using Xunit;

namespace SnAbp.TenantManagement
{
    public class Localization_Tests : SnAbpTenantManagementDomainTestBase
    {
        private readonly IStringLocalizer<AbpTenantManagementResource> _stringLocalizer;

        public Localization_Tests()
        {
            _stringLocalizer = GetRequiredService<IStringLocalizer<AbpTenantManagementResource>>();
        }

        [Fact]
        public void Test()
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("Tenant '{0}' will be deleted. Do you confirm that?");
        }
    }
}
