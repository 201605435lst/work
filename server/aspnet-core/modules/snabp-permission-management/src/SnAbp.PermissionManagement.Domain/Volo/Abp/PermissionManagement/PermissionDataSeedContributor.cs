using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace SnAbp.PermissionManagement
{
    public class PermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected IPermissionDataSeeder PermissionDataSeeder { get; }

        public PermissionDataSeedContributor(
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionDataSeeder permissionDataSeeder,
            ICurrentTenant currentTenant)
        {
            PermissionDefinitionManager = permissionDefinitionManager;
            PermissionDataSeeder = permissionDataSeeder;
            CurrentTenant = currentTenant;
        }

        public virtual Task SeedAsync(DataSeedContext context)
        {
            var multiTenancySide = CurrentTenant.GetMultiTenancySide();
            var permissionNames = PermissionDefinitionManager
                .GetPermissions()
                .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
                .Select(p => p.Name)
                .ToArray();

            //return PermissionDataSeeder.SeedAsync(
            //    RolePermissionValueProvider.ProviderName,
            //    "admin",
            //    permissionNames,
            //    context.TenantId
            //);
            // TODO 需要给 默认添加的种子数据添加已有的绝色的id，需要通过查询获取的。
            return PermissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                Guid.Parse("5C146D09-2530-4545-8673-B518DB3F94FC"),
                permissionNames,
                context.TenantId
            );
        }
    }
}
