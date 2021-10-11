using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using SnAbp.Users.MongoDB;

namespace SnAbp.Identity.MongoDB
{
    [DependsOn(
        typeof(SnAbpIdentityDomainModule),
        typeof(SnAbpUsersMongoDbModule)
        )]
    public class SnAbpIdentityMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<SnAbpIdentityMongoDbContext>(options =>
            {
                options.AddRepository<IdentityUser, MongoIdentityUserRepository>();
                options.AddRepository<IdentityRole, MongoIdentityRoleRepository>();
                options.AddRepository<IdentityClaimType, MongoIdentityRoleRepository>();
                options.AddRepository<Organization, MongoIdentityRoleRepository>();
            });
        }
    }
}