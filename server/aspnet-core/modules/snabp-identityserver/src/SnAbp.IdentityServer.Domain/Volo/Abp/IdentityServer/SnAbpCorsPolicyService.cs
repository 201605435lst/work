using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using SnAbp.IdentityServer.Clients;

namespace SnAbp.IdentityServer
{
    public class SnAbpCorsPolicyService : ICorsPolicyService
    {
        public ILogger<SnAbpCorsPolicyService> Logger { get; set; }
        protected IHybridServiceScopeFactory HybridServiceScopeFactory { get; }
        protected IDistributedCache<AllowedCorsOriginsCacheItem> Cache { get; }

        public SnAbpCorsPolicyService(
            IDistributedCache<AllowedCorsOriginsCacheItem> cache,
            IHybridServiceScopeFactory hybridServiceScopeFactory)
        {
            Cache = cache;
            HybridServiceScopeFactory = hybridServiceScopeFactory;
            Logger = NullLogger<SnAbpCorsPolicyService>.Instance;
        }

        public virtual async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var cacheItem = await Cache.GetOrAddAsync(AllowedCorsOriginsCacheItem.AllOrigins, CreateCacheItemAsync);

            var isAllowed = cacheItem.AllowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            if (!isAllowed)
            {
                Logger.LogWarning($"Origin is not allowed: {origin}");
            }

            return isAllowed;
        }

        protected virtual async Task<AllowedCorsOriginsCacheItem> CreateCacheItemAsync()
        {
            // doing this here and not in the ctor because: https://github.com/aspnet/AspNetCore/issues/2377
            using (var scope = HybridServiceScopeFactory.CreateScope())
            {
                var clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

                return new AllowedCorsOriginsCacheItem
                {
                    AllowedOrigins = (await clientRepository.GetAllDistinctAllowedCorsOriginsAsync()).ToArray()
                };
            }
        }
    }
}
