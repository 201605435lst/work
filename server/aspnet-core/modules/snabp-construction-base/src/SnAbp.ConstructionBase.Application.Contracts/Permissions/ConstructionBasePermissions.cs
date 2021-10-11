using Volo.Abp.Reflection;

namespace SnAbp.ConstructionBase.Permissions
{
    public class ConstructionBasePermissions
    {
        public const string GroupName = "ConstructionBase";

        public static class Worker {
            public const string Default = GroupName + ".Worker";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Detail = Default + ".Detail";
            public const string Delete = Default + ".Delete";
        }

        public static class EquipmentTeam
        {
            public const string Default = GroupName + ".EquipmentTeam";
            public const string Create  = Default   + ".Create";
            public const string Update  = Default   + ".Update";
            public const string Detail  = Default   + ".Detail";
            public const string Delete  = Default   + ".Delete";
        }

        public static class Material
        {
            public const string Default = GroupName + ".Material";
            public const string Create  = Default   + ".Create";
            public const string Update  = Default   + ".Update";
            public const string Detail  = Default   + ".Detail";
            public const string Delete  = Default   + ".Delete";
        }

        public static class Procedure
        {
            public const string Default = GroupName + ".Procedure";
            
            public const string Create  = Default   + ".Create";
            public const string Update  = Default   + ".Update";
            public const string Detail  = Default   + ".Detail";
            public const string Delete  = Default   + ".Delete";
        }

        public static class SubItem
        {
            public const string Default = GroupName + ".SubItem";
            
            public const string Create  = Default   + ".Create";
            public const string Update  = Default   + ".Update";
            public const string Detail  = Default   + ".Detail";
            public const string Delete  = Default   + ".Delete";
        }






        public static class Standard
        {
            public const string Default = GroupName + ".Standard";

			public const string Create = Default + ".Create";
			public const string Update = Default + ".Update";
			public const string Detail = Default + ".Detail";
			public const string Delete = Default + ".Delete";
		}


        public static class Section
        {
            public const string Default = GroupName + ".Section";

			public const string Create = Default + ".Create";
			public const string Update = Default + ".Update";
			public const string Detail = Default + ".Detail";
			public const string Delete = Default + ".Delete";
		}

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ConstructionBasePermissions));
        }
    }
}
