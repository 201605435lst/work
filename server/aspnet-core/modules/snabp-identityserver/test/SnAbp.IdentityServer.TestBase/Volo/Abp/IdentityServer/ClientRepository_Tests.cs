using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SnAbp.IdentityServer.Clients;
using Volo.Abp.Modularity;
using Xunit;

namespace SnAbp.IdentityServer
{
    public abstract class ClientRepository_Tests<TStartupModule> : SnAbpIdentityServerTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IClientRepository clientRepository { get; }

        protected ClientRepository_Tests()
        {
            clientRepository = ServiceProvider.GetRequiredService<IClientRepository>();
        }

        [Fact]
        public async Task FindByCliendIdAsync()
        {
            (await clientRepository.FindByCliendIdAsync("ClientId2")).ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllDistinctAllowedCorsOriginsAsync()
        {
            var origins = await clientRepository.GetAllDistinctAllowedCorsOriginsAsync();
            origins.Any().ShouldBeTrue();
        }
    }
}
