using SnAbp.Alarm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Alarm.Permissions
{
    public class AlarmPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(AlarmPermissions.GroupName, L("Permission:Alarm"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AlarmResource>(name);
        }
    }
}