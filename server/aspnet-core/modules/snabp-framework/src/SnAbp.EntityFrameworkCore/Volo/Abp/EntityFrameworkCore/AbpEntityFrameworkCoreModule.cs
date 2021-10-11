using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SnAbp.Domain;
using SnAbp.Uow.EntityFrameworkCore;

using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace SnAbp.EntityFrameworkCore
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.PreConfigure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions
                        .ConfigureWarnings(warnings =>
                        {
                            warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning);
                        });
                });
            });

            context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
        }
    }
}
