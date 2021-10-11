
using SnAbp.Message.Bpm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Message.Bpm.Permissions
{
    public class BpmPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(BpmPermissions.GroupName, L("Permission:Bpm"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MessageBpmResource>(name);
        }
    }
}