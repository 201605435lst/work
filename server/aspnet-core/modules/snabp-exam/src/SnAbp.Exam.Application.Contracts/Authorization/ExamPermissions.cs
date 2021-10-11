using Volo.Abp.Reflection;

namespace SnAbp.Exam.Authorization
{
    public class ExamPermissions
    {
        public const string GroupName = "AbpExam";

        public static class Category
        {
            public const string Default = GroupName + ".Category";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Paper
        {
            public const string Default = GroupName + ".Paper";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class PaperTemplate
        {
            public const string Default = GroupName + ".PaperTemplate";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class KnowledgePoint
        {
            public const string Default = GroupName + ".KnowledgePoint";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Question
        {
            public const string Default = GroupName + ".Question";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ExamPermissions));
        }
    }
}