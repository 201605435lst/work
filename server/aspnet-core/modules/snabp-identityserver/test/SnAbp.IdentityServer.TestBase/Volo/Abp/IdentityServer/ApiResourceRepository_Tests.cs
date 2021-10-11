using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SnAbp.IdentityServer.ApiResources;
using Volo.Abp.Modularity;
using Xunit;

namespace SnAbp.IdentityServer
{
    public abstract class ApiResourceRepository_Tests<TStartupModule> : SnAbpIdentityServerTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IApiResourceRepository apiResourceRepository { get; }

        public ApiResourceRepository_Tests()
        {
            apiResourceRepository = ServiceProvider.GetRequiredService<IApiResourceRepository>();
        }

        [Fact]
        public async Task FindByNormalizedNameAsync()
        {
            (await apiResourceRepository.FindByNameAsync("NewApiResource2")).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetListByScopesAsync()
        {
            (await apiResourceRepository.GetListByScopesAsync(new[] { "NewApiResource2", "NewApiResource3" })).Count.ShouldBe(2);
        }
    }
}
