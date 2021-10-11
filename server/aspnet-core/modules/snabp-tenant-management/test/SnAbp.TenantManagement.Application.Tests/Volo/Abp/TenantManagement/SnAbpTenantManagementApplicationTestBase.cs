using System;
using SnAbp.TenantManagement.EntityFrameworkCore;

namespace SnAbp.TenantManagement
{
    public abstract class SnAbpTenantManagementApplicationTestBase : TenantManagementTestBase<SnAbpTenantManagementApplicationTestModule>
    {
        protected virtual void UsingDbContext(Action<ITenantManagementDbContext> action)
        {
            using (var dbContext = GetRequiredService<ITenantManagementDbContext>())
            {
                action.Invoke(dbContext);
            }
        }

        protected virtual T UsingDbContext<T>(Func<ITenantManagementDbContext, T> action)
        {
            using (var dbContext = GetRequiredService<ITenantManagementDbContext>())
            {
                return action.Invoke(dbContext);
            }
        }
    }
}
