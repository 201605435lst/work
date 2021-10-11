using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Identity.MongoDB
{
    [ConnectionStringName(SnAbpIdentityDbProperties.ConnectionStringName)]
    public interface ISnAbpIdentityMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<IdentityUser> Users { get; }

        IMongoCollection<IdentityRole> Roles { get; }

        IMongoCollection<IdentityClaimType> ClaimTypes { get; }

        IMongoCollection<Organization> Organizations { get; }
    }
}