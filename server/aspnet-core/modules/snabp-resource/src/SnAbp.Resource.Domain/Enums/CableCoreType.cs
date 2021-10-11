using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Resource.Enums
{


    /// <summary>
    /// 电缆类型 1:电缆芯，2:光缆芯
    /// </summary>
    public enum CableCoreType
    {
        [Description("电缆芯")]
        Electric = 1,

        [Description("光缆芯")]
        Optical = 2,
    }
}
