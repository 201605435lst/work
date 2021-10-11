using SnAbp.Exam.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SnAbp.Exam.Authorization
{
    public class ExamPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {

            var moduleGroup = context.AddGroup(ExamPermissions.GroupName, L(ExamPermissions.GroupName));


            var categoryPerm = moduleGroup.AddPermission(ExamPermissions.Category.Default, L("Permission:CategoryManagement"));
            categoryPerm.AddChild(ExamPermissions.Category.Create, L("Permission:Create"));
            categoryPerm.AddChild(ExamPermissions.Category.Delete, L("Permission:Delete"));
            categoryPerm.AddChild(ExamPermissions.Category.Update, L("Permission:Update"));
            categoryPerm.AddChild(ExamPermissions.Category.ManagePermissions, L("Permission:ChangePermissions"));

            var paperPerm = moduleGroup.AddPermission(ExamPermissions.Paper.Default, L("Permission:PaperManagement"));
            paperPerm.AddChild(ExamPermissions.Paper.Create, L("Permission:Create"));
            paperPerm.AddChild(ExamPermissions.Paper.Delete, L("Permission:Delete"));
            paperPerm.AddChild(ExamPermissions.Paper.Update, L("Permission:Update"));
            paperPerm.AddChild(ExamPermissions.Paper.ManagePermissions, L("Permission:ChangePermissions"));

            var paperTemplatePerm = moduleGroup.AddPermission(ExamPermissions.PaperTemplate.Default, L("Permission:PaperTemplateManagement"));
            paperTemplatePerm.AddChild(ExamPermissions.PaperTemplate.Create, L("Permission:Create"));
            paperTemplatePerm.AddChild(ExamPermissions.PaperTemplate.Delete, L("Permission:Delete"));
            paperTemplatePerm.AddChild(ExamPermissions.PaperTemplate.Update, L("Permission:Update"));
            paperTemplatePerm.AddChild(ExamPermissions.PaperTemplate.ManagePermissions, L("Permission:ChangePermissions"));

            var knowledgePointPerm = moduleGroup.AddPermission(ExamPermissions.KnowledgePoint.Default, L("Permission:KnowledgePointManagement"));
            knowledgePointPerm.AddChild(ExamPermissions.KnowledgePoint.Create, L("Permission:Create"));
            knowledgePointPerm.AddChild(ExamPermissions.KnowledgePoint.Delete, L("Permission:Delete"));
            knowledgePointPerm.AddChild(ExamPermissions.KnowledgePoint.Update, L("Permission:Update"));
            knowledgePointPerm.AddChild(ExamPermissions.KnowledgePoint.ManagePermissions, L("Permission:ChangePermissions"));

            var questionPerm = moduleGroup.AddPermission(ExamPermissions.Question.Default, L("Permission:CategoryManagement"));
            questionPerm.AddChild(ExamPermissions.Question.Create, L("Permission:Create"));
            questionPerm.AddChild(ExamPermissions.Question.Delete, L("Permission:Delete"));
            questionPerm.AddChild(ExamPermissions.Question.Update, L("Permission:Update"));
            questionPerm.AddChild(ExamPermissions.Question.ManagePermissions, L("Permission:ChangePermissions"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ExamResource>(name);
        }
    }
}