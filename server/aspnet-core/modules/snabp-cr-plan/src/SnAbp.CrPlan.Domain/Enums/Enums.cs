using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.CrPlan.Enumer
{
    /// <summary>
    /// 年月表类型
    /// </summary>
    public enum YearMonthPlanType
    {
        年表 = 1,
        月表 = 2,
        年度月表 = 3,
    }

    /// <summary>
    /// 统计使用年月表类型
    /// </summary>
    public enum YearMonthPlanStatisticalType
    {
        年表 = 1,
        月表 = 2
    }

    /// <summary>
    /// 统计使用月表类型
    /// </summary>
    public enum MonthPlanStatisticalType
    {
        月表 = 2,
        年度月表 = 3
    }

    /// <summary>
    /// 年月表状态
    /// </summary>
    public enum YearMonthPlanState
    {
        未提交 = 0,
        待审核 = 1,
        审核中 = 2,
        审核通过 = 3,
        审核驳回 = 4,
    }

    /// <summary>
    /// 天窗类型
    /// </summary>
    public enum SkyligetType
    {
        垂直天窗,
        综合天窗,
        天窗点外,
        各网管,
        其他
    }

    /// <summary>
    /// 年表导出列名
    /// </summary>
    public enum YearPlanExcelCol
    {
        Id,
        序号,
        维修类别,
        设备名称,
        执行单位,
        编制执行单位,
        设备处所,
        工作内容,
        天窗类型,
        单位,
        总设备数量,
        年计划总数量,
        每年次数,
        一月,
        二月,
        三月,
        四月,
        五月,
        六月,
        七月,
        八月,
        九月,
        十月,
        十一月,
        十二月
    }

    /// <summary>
    /// 月表导出列名
    /// </summary>
    public enum MonthPlanExcelCol
    {
        Id,
        序号,
        维修类别,
        设备名称,
        编制执行单位,
        执行单位,
        设备处所,
        工作内容,
        天窗类型,
        单位,
        数量,
        每月次数,
    }

    /// <summary>
    /// 年月表导出列名
    /// </summary>
    public enum MonthOfYearPlanExcelCol
    {
        Id,
        序号,
        维修类别,
        设备名称,
        编制执行单位,
        执行单位,
        设备处所,
        工作内容,
        天窗类型,
        单位,
        数量
    }

    /// <summary>
    /// 添加待选计划类型
    /// </summary>
    public enum SelectablePlanType
    {
        [Description("年表")]
        Year = 1,
        [Description("半年表")]
        HalfYaer = 2,
        [Description("季度表")]
        QuarterYear = 3,
        [Description("月表")]
        Month = 4,
    }

    /// <summary>
    /// 计划完成状态
    /// </summary>
    public enum PlanFinishState
    {
        [Description("未完成")]
        Unfinished = 0,
        [Description("已完成")]
        Complete = 1,
        [Description("全部")]
        All = 2
    }
    public enum StatisticalRepairType
    {
        [Description("集中检修")]
        CENTRALIZED = 1,
        [Description("日常检修")]
        DAILY = 2,
        [Description("全部")]
        All = 3,
        [Description("其他")]
        OTHER = 4
    }
}
