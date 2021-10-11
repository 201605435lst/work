using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Report.Enums
{
    public enum ReportType
    {
        [Description("全部")]
        AllRepot = 0,
        [Description("日报")]
        DayRepot = 1,
        [Description("周报")]
        WeekReport = 2,
        [Description("月报")]
        MonthReport = 3
    }
    public enum ReportExcelCol
    {
        序号,
        项目名称,
        标题,
        类型,
        日期,
        工作计划,
        工作记录,
        通知人,

    }
}
