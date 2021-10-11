using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using SnAbp.Users.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SnAbp.Identity.EntityFrameworkCore
{
    [DependsOn(
        typeof(SnAbpIdentityDomainModule), 
        typeof(SnAbpUsersEntityFrameworkCoreModule))]
    public class SnAbpIdentityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityDbContext>(options =>
            {
                options.AddDefaultRepositories<IIdentityDbContext>(true);
                options.AddRepository<IdentityUser, EfCoreIdentityUserRepository>();
                options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
                options.AddRepository<IdentityClaimType, EfCoreIdentityClaimTypeRepository>();
                options.AddRepository<Organization, OrganizationRepository>();
                options.AddRepository<DataDictionary, EfCoreDataDictionaryRepository>();
                options.AddRepository<OrganizationRltRole, EfCoreOrganizationRltRoleRepository>();
                options.AddRepository<IdentityUserRltOrganization, EfCoreIdentityUserRltOrganizationRepository>();

                options.Entity<Organization>(x => x.DefaultWithDetailsFunc = q => q
                       .Include(x => x.Parent)
                   );

            });
        }
    }
}