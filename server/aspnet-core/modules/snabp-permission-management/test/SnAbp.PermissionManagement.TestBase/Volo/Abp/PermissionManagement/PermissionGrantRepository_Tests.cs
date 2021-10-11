using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Xunit;

namespace SnAbp.PermissionManagement
{
    public abstract class PermissionGrantRepository_Tests<TStartupModule> : PermissionManagementTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected PermissionGrantRepository_Tests()
        {
            PermissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        }

        [Fact]
        public async Task FindAsync()
        {
            (await PermissionGrantRepository.FindAsync("MyPermission1", UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id)).ShouldNotBeNull();

            (await PermissionGrantRepository.FindAsync("Undefined-Permission", UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id)).ShouldBeNull();
            (await PermissionGrantRepository.FindAsync("MyPermission1", "Undefined-Provider", Guid.NewGuid())).ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var permissionGrants = await PermissionGrantRepository.GetListAsync(UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id);

            permissionGrants.ShouldContain(p => p.Name == "MyPermission1");
        }
    }
}
