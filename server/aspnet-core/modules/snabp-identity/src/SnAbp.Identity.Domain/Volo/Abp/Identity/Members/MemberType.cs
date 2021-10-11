using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Identity
{
    public enum MemberType
    {
        Organization = 1, //组织
        Role = 2, //角色
        User = 3, //用户


        // 动态组织扩展
        DynamicOrgLevel1=21,
        DynamicOrgLevel2=22,
        DynamicOrgLevel3=23,
        DynamicOrgLevel4=24,
        DynamicOrgLevel5=25,
        DynamicOrgLevel6=26,
        DynamicOrgLevel7=27,
        DynamicOrgLevel8=28,
        DynamicOrgLevel9=29,

        // 动态领导（职位）扩展 TODO
    }
}
