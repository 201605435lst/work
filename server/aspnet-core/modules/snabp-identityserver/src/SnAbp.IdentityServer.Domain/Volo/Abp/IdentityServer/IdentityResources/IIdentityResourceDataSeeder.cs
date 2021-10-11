using System.Threading.Tasks;

namespace SnAbp.IdentityServer.IdentityResources
{
    public interface IIdentityResourceDataSeeder
    {
        Task CreateStandardResourcesAsync();
    }
}