using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;
using SnAbp.IdentityServer.ApiResources;
using SnAbp.IdentityServer.Clients;
using SnAbp.IdentityServer.Devices;
using SnAbp.IdentityServer.Grants;
using SnAbp.IdentityServer.IdentityResources;

namespace SnAbp.IdentityServer.EntityFrameworkCore
{
    [ConnectionStringName(SnAbpIdentityServerDbProperties.ConnectionStringName)]
    public interface IIdentityServerDbContext : IEfCoreDbContext
    {
        DbSet<ApiResource> ApiResources { get; set; }

        DbSet<ApiSecret> ApiSecrets { get; set; }

        DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

        DbSet<ApiScope> ApiScopes { get; set; }

        DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

        DbSet<IdentityResource> IdentityResources { get; set; }

        DbSet<IdentityClaim> IdentityClaims { get; set; }

        DbSet<Client> Clients { get; set; }

        DbSet<ClientGrantType> ClientGrantTypes { get; set; }

        DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }

        DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }

        DbSet<ClientScope> ClientScopes { get; set; }

        DbSet<ClientSecret> ClientSecrets { get; set; }

        DbSet<ClientClaim> ClientClaims { get; set; }

        DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }

        DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }

        DbSet<ClientProperty> ClientProperties { get; set; }

        DbSet<PersistedGrant> PersistedGrants { get; set; }

        DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
    }
}
