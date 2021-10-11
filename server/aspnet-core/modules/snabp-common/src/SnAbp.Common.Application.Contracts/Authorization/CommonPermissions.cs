using Volo.Abp.Reflection;

namespace SnAbp.Common.Authorization
{
    public class CommonPermissions
    {
        //public const string GroupName = "Orgs";
        //public const string Orgs_Organization = "Orgs.Organization";
        //public const string Orgs_Organization_Create = "Orgs.Organization.Create";
        //public const string Orgs_Organization_Update = "Orgs.Organization.Update";
        //public const string Orgs_Organization_Delete = "Orgs.Organization.Delete";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CommonPermissions));
        }
    }
}