using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.AlertsDemo
{
    [Widget]
    public class AlertsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/AlertsDemo/Default.cshtml";

        public IViewComponentResult Invoke()
        {
            return View(ViewPath);
        }
    }
}