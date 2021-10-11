using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.TenantManagement.EntityFrameworkCore
{
    public class LazyLoad_Tests : LazyLoad_Tests<SnAbpTenantManagementEntityFrameworkCoreTestModule>
    {
        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.Configure<AbpDbContextOptions>(options =>
            {
                options.PreConfigure<TenantManagementDbContext>(context =>
                {
                    context.DbContextOptions.UseLazyLoadingProxies();
                });
            });
        }
    }
}
