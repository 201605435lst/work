using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace SnAbp.Oa
{
    [Dependency(ReplaceServices = true)]
    public class OaBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Oa";
    }
}
