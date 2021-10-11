using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Material.Enums
{
    public enum TestingType
    {
        [Description("送检")]
        Inspect = 1,
        [Description("自检")]
        SelfInspection = 2,
    }
    public enum TestingStatus
    {
        [Description("待验收")]
        ForAcceptance = 1,
        [Description("已验收")]
        Approved = 2,
    }
    public enum TestState
    {
        [Description("合格")]
        Qualified = 1,
        [Description("不合格")]
        Disqualification = 2,
    }
}
