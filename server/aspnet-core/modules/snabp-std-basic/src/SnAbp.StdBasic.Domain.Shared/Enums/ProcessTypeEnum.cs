using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.StdBasic.Enums
{
    /// <summary>
    /// 工序类别
    /// </summary>
    public enum ProcessTypeEnum
    {
        [Description("管理任务")]
        ManagemenetTask = 1,
        [Description("施工任务")]
        ConstructionTask = 2
    }
}
