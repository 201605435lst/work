using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace SnAbp.StdBasic.Enums
{
    public enum RepairType
    {
        [Description("集中检修")]
        CENTRALIZED = 1,
        [Description("日常检修")]
        DAILY = 2,
        [Description("重点检修")]
        KEY = 3,
        [Description("其他")]
        OTHER = 4
    }

    public enum RepairTestType
    {
        [Description("数字")]
        NUMBER = 1,
        [Description("字符")]
        STRING = 2,
        [Description("表格")]
        EXCEL = 3
    }
    public enum RepairPeriodUnit
    {
        [Description("其他")]
        OTHER = 1,
        [Description("年")]
        YEAR = 2,
        [Description("月")]
        MONTH = 3
    }
}
