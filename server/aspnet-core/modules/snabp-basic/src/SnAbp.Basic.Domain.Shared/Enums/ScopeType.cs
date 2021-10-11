using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Basic.Enums
{
    /// <summary>
    /// 范围类型
    /// </summary>
    public enum ScopeType
    {
        // 组织范围
        Organization = 1,
        
        // 线路范围
        Railway = 2,

        // 车站范围
        Station = 3,

        // 安装位置范围
        InstallationSite = 4
    }
}
