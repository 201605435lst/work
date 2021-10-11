using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 年表统计Dto
    /// </summary>
    public class YearMonthPlanYearStatisticalDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 维修项主键
        /// </summary>
        public Guid RepairDetailsId { get; set; }

        /// <summary>
        /// 维修项序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 执行单位
        /// </summary>
        public string ExecutableUnitStr { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string RepairGroup { get; set; }

        /// <summary>
        /// 类别（集中检修/日常检修）
        /// </summary>
        public int RepairType { get; set; }

        /// <summary>
        /// 设备名称(维修项设备)
        /// </summary>
        public string DeviceName { get; set; }


        /// <summary>
        /// 工作内容
        /// </summary>
        public string RepairContent { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public int DeviceCount { get; set; }

        /// <summary>
        /// 设备总数量
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 次数
        /// </summary>
        public string Times { get; set; }


        /// <summary>
        /// 年计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 列1(月份/日)
        /// </summary>
        public decimal Col_1 { get; set; }

        /// <summary>
        /// 列2(月份/日)
        /// </summary>
        public decimal Col_2 { get; set; }

        /// <summary>
        /// 列3(月份/日)
        /// </summary>
        public decimal Col_3 { get; set; }

        /// <summary>
        /// 列4(月份/日)
        /// </summary>
        public decimal Col_4 { get; set; }

        /// <summary>
        /// 列5(月份/日)
        /// </summary>
        public decimal Col_5 { get; set; }

        /// <summary>
        /// 列6(月份/日)
        /// </summary>
        public decimal Col_6 { get; set; }

        /// <summary>
        /// 列7(月份/日)
        /// </summary>
        public decimal Col_7 { get; set; }

        /// <summary>
        /// 列8(月份/日)
        /// </summary>
        public decimal Col_8 { get; set; }

        /// <summary>
        /// 列9(月份/日)
        /// </summary>
        public decimal Col_9 { get; set; }

        /// <summary>
        /// 列10(月份/日)
        /// </summary>
        public decimal Col_10 { get; set; }

        /// <summary>
        /// 列11(月份/日)
        /// </summary>
        public decimal Col_11 { get; set; }

        /// <summary>
        /// 列12(月份/日)
        /// </summary>
        public decimal Col_12 { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        /// <summary>
        /// 子项
        /// </summary>
        public List<YearMonthPlanYearStatisticalChildDto> ChildItems { get; set; } = new List<YearMonthPlanYearStatisticalChildDto>();
    }

    public class YearMonthPlanYearStatisticalChildDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 负责单位
        /// </summary>
        public Guid ResponsibleUnit { get; set; }

        /// <summary>
        /// 负责单位Str
        /// </summary>
        public string ResponsibleUnitStr { get; set; }

        /// <summary>
        /// 设备处所
        /// </summary>
        public string EquipmentLocation { get; set; }


        /// <summary>
        /// 天窗类型(多个用逗号分隔)
        /// </summary>
        public string SkyligetType { get; set; }


        /// <summary>
        /// 年计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 次数
        /// </summary>
        public string Times { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 列1(月份/日)
        /// </summary>
        public decimal Col_1 { get; set; }

        /// <summary>
        /// 列2(月份/日)
        /// </summary>
        public decimal Col_2 { get; set; }

        /// <summary>
        /// 列3(月份/日)
        /// </summary>
        public decimal Col_3 { get; set; }

        /// <summary>
        /// 列4(月份/日)
        /// </summary>
        public decimal Col_4 { get; set; }

        /// <summary>
        /// 列5(月份/日)
        /// </summary>
        public decimal Col_5 { get; set; }

        /// <summary>
        /// 列6(月份/日)
        /// </summary>
        public decimal Col_6 { get; set; }

        /// <summary>
        /// 列7(月份/日)
        /// </summary>
        public decimal Col_7 { get; set; }

        /// <summary>
        /// 列8(月份/日)
        /// </summary>
        public decimal Col_8 { get; set; }

        /// <summary>
        /// 列9(月份/日)
        /// </summary>
        public decimal Col_9 { get; set; }

        /// <summary>
        /// 列10(月份/日)
        /// </summary>
        public decimal Col_10 { get; set; }

        /// <summary>
        /// 列11(月份/日)
        /// </summary>
        public decimal Col_11 { get; set; }

        /// <summary>
        /// 列12(月份/日)
        /// </summary>
        public decimal Col_12 { get; set; }
    }
}
