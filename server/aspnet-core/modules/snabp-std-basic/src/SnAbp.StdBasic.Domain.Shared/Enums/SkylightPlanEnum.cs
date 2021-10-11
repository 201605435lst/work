using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.StdBasic.Enums
{
    public enum SkylightPlanRepairLevel
    {
        [Description("天窗点内I级维修")]
        LeveIRepairInSkylight = 1,
        [Description("天窗点内II级维修")]
        LeveIIRepairInSkylight = 2,
        [Description("天窗点外I级维修")]
        LeveIRepairOutSkylight = 3,
        [Description("天窗点外II级维修")]
        LeveIIRepairOutSkylight = 4
    }

}
