using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Basic.Enums
{
    public enum StationType
    {
        [Description("站点")]
        STATION =0,
        [Description("区间")]
        INTERVAL = 1
    }

    public enum RelateRailwayType
    {
        [Description("单线")]
        SINGLELINK = 0,
        [Description("上行")]
        UPLINK = 1,
        [Description("下行")]
        DOWNLINK = 2,
        [Description("上下行")]
        UPANDDOWN = 3
    }
}
