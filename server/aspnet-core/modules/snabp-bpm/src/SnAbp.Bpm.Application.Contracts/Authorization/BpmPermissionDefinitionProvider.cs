using SnAbp.Bpm.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Bpm.Authorization
{
    public class BpmPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var bpmGroup = context.AddGroup(BpmPermissions.GroupName, L("Permission:Bpm"));

            var workflowTemplatePermission = bpmGroup.AddPermission(BpmPermissions.WorkflowTemplate.Default, L("Permission:WorkflowTemplates"));
            workflowTemplatePermission.AddChild(BpmPermissions.WorkflowTemplate.Create, L("Permission:Create"));
            workflowTemplatePermission.AddChild(BpmPermissions.WorkflowTemplate.Update, L("Permission:Update"));
            workflowTemplatePermission.AddChild(BpmPermissions.WorkflowTemplate.Delete, L("Permission:Delete"));
            workflowTemplatePermission.AddChild(BpmPermissions.WorkflowTemplate.Detail, L("Permission:Detail"));
            workflowTemplatePermission.AddChild(BpmPermissions.WorkflowTemplate.PublishState, L("Permission:PublishState"));
            workflowTemplatePermission.AddChild(BpmPermissions.WorkflowTemplate.DataStatistic, L("Permission:DataStatistic"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BpmResource>(name);
        }
    }
}