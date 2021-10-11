using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Resource.Enums
{


    /// <summary>
    /// 电缆敷设类型
    /// </summary>
    public enum CableLayType
    {
        [Description("管道")]
        Conduit = 1,

        [Description("架空")]
        Overhead = 2,

        [Description("直埋")]
        Bury = 3,

        [Description("室内槽道及竖井")]
        InnerChannelFlow = 4,

        [Description("室外槽道")]
        OuterChannelFlow = 5,
    }
}
