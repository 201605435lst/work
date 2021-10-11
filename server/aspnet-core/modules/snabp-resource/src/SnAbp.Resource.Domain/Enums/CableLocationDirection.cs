using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Resource.Enums
{


    /// <summary>
    /// 设备运行状态 1:电缆芯，2:光缆芯
    /// </summary>
    public enum CableLocationDirection
    {
        [Description("水平方向")]
        Horizontal = 1,

        [Description("垂直方向")]
        Vertical = 2,

        [Description("直线距离")]
        Straight = 3,
    }
}
