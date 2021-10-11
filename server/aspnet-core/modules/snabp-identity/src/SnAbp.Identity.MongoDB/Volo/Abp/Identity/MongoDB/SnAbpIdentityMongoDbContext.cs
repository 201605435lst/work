using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Identity.MongoDB
{
    [ConnectionStringName(SnAbpIdentityDbProperties.ConnectionStringName)]
    public class SnAbpIdentityMongoDbContext : AbpMongoDbContext, ISnAbpIdentityMongoDbContext
    {
        public IMongoCollection<IdentityUser> Users => Collection<IdentityUser>();

        public IMongoCollection<IdentityRole> Roles => Collection<IdentityRole>();

        public IMongoCollection<IdentityClaimType> ClaimTypes => Collection<IdentityClaimType>();

        public IMongoCollection<Organization> Organizations => Collection<Organization>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureIdentity();
        }
    }
}