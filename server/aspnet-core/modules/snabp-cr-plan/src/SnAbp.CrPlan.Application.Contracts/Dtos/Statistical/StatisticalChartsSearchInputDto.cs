using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using System;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 统计图表查询条件
    /// </summary>
    public class StatisticalChartsSearchInputDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 维修项标签
        /// </summary>
        public string RepairTag { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 时间（到月）
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 月完成率计划类型(年表月计划、月表)
        /// </summary>
        public YearMonthPlanStatisticalType MonthPlanType { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public Guid EquipmentTypeId { get; set; }
        public string RepairTagKey { get ; set ; }
    }

    /// <summary>
    /// 月表完成率查询条件
    /// </summary>
    public class MonthFinishSearchInputDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 完成情况计划类型(年表、月表)
        /// </summary>
        public YearMonthPlanStatisticalType MonthPlanType { get; set; }


        /// <summary>
        /// 时间（到月）
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 状态（已完成、未完成）
        /// 只月完成情况统计使用
        /// </summary>
        public PlanFinishState State { get; set; }
        public string RepairTagKey { get ; set ; }
    }

    /// <summary>
    /// 年表完成率查询条件
    /// </summary>
    public class YearFinishSearchInputDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 维修类型(集中检修/日常检修)
        /// </summary>
        public StatisticalRepairType RepairlType { get; set; }

        /// <summary>
        /// 时间（到月）
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 关键字(【设备类型】【设备名称】【工作内容】模糊搜索)
        /// </summary>
        public string KeyWord { get; set; }
        public string RepairTagKey { get ; set ; }
    }

    /// <summary>
    /// 计划状态追踪查询条件
    /// </summary>
    public class PlanStateTrackingSearchInputDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 时间（到月）
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 计划类型(年表、月表)
        /// </summary>
        public YearMonthPlanStatisticalType MonthPlanType { get; set; }

        /// <summary>
        /// 年月表序号
        /// </summary>
        public string SequenceNumber { get; set; }
        public string RepairTagKey { get ; set ; }
    }

    /// <summary>
    /// 单项完成情况查询条件
    /// </summary>
    public class SingleCompleteSearchInputDto : IRepairTagKeyDto
    {
        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 完成情况计划类型(年表、月表)
        /// </summary>
        public YearMonthPlanStatisticalType MonthPlanType { get; set; }


        /// <summary>
        /// 时间（到月）
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        ///序号
        /// </summary>
        public string Number { get; set; }
        public string RepairTagKey { get ; set ; }
    }

}
