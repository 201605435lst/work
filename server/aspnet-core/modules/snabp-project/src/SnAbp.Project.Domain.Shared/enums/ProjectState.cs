using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Project.enums
{
    public enum ProjectState
    {
        [Description("建设中")]
        Building = 1,

        [Description("竣工")]
        Finshed = 2,

        [Description("验收")]
        Acceptance = 3,

        [Description("终止")]
        Stop = 4,

        [Description("待勘察")]
        WaitSurvey = 5,

        [Description("未开始")]
        NoStart = 6,

        [Description("开始")]
        Start = 7,
    }
}
