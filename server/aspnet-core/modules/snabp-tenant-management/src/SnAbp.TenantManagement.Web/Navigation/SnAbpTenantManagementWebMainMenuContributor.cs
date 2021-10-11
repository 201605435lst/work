using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SnAbp.TenantManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace SnAbp.TenantManagement.Web.Navigation
{
    public class SnAbpTenantManagementWebMainMenuContributor : IMenuContributor
    {
        public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return;
            }

            var administrationMenu = context.Menu.GetAdministration();

            var l = context.GetLocalizer<AbpTenantManagementResource>();

            var tenantManagementMenuItem = new ApplicationMenuItem(TenantManagementMenuNames.GroupName, l["Menu:TenantManagement"], icon: "fa fa-users");
            administrationMenu.AddItem(tenantManagementMenuItem);

            if (await context.IsGrantedAsync(TenantManagementPermissions.Tenants.Default))
            {
                tenantManagementMenuItem.AddItem(new ApplicationMenuItem(TenantManagementMenuNames.Tenants, l["Tenants"], url: "~/TenantManagement/Tenants"));
            }
        }
    }
}
