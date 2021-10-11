using SnAbp.ComponentTrack.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.ComponentTrack.Permissions
{
    public class ComponentTrackPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
           

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ComponentTrackResource>(name);
        }
    }
}