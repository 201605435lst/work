using Volo.Abp.Reflection;

namespace SnAbp.Bpm.Authorization
{
    public class BpmPermissions
    {
        public const string GroupName = "AbpBpm";

        public static class WorkflowTemplate
        {
            public const string Default = GroupName + ".WorkflowTemplates";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Detail = Default + ".Detail"; 
            public const string PublishState = Default + ".PublishState";
            public const string DataStatistic = Default + ".DataStatistic";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BpmPermissions));
        }
    }
}