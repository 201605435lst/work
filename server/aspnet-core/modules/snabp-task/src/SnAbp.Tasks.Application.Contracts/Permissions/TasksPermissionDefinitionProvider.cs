using SnAbp.Tasks.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Tasks.Permissions
{
    public class TasksPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var taskGroup = context.AddGroup(TasksPermissions.GroupName, L(TasksPermissions.GroupName));

            var taskPerm = taskGroup.AddPermission(TasksPermissions.Task.Default, L("Permission:Tasks"));
            taskPerm.AddChild(TasksPermissions.Task.Create, L("Permission:Create"));
            taskPerm.AddChild(TasksPermissions.Task.Delete, L("Permission:Delete"));
            taskPerm.AddChild(TasksPermissions.Task.Update, L("Permission:Update"));
            taskPerm.AddChild(TasksPermissions.Task.Detail, L("Permission:Detail"));
            taskPerm.AddChild(TasksPermissions.Task.Export, L("Permission:Export"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TasksResource>(name);
        }
    }
}