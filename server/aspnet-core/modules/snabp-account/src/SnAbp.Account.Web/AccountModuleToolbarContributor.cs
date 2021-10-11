using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Account.Web.Modules.Account.Components.Toolbar.UserLoginLink;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Users;

namespace SnAbp.Account.Web
{
    public class AccountModuleToolbarContributor : IToolbarContributor
    {
        public virtual Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name != StandardToolbars.Main)
            {
                return Task.CompletedTask;
            }

            //TODO: Currently disabled!
            //if (!context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
            //{
            //    context.Toolbar.Items.Add(new ToolbarItem(typeof(UserLoginLinkViewComponent)));
            //}

            return Task.CompletedTask;
        }
    }
}
