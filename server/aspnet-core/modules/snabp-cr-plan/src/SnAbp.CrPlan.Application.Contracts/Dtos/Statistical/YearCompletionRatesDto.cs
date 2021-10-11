using SnAbp.Basic.Enums;
using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 年表完成率实体
    /// </summary>
    public class YearCompletionRatesDto : IRepairTagDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 设备类型(维修项)
        /// </summary>
        public string RepairGroup { get; set; }

        /// <summary>
        /// 类别（集中检修/日常检修）
        /// </summary>
        public RepairType RepairType { get; set; }

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
        /// 每年次数
        /// </summary>
        public string Times { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 累计完成数量
        /// </summary>
        public decimal CumulativeFinishedCount { get; set; }

        /// <summary>
        /// 月完成数量
        /// </summary>
        public decimal MonthFinishedCount { get; set; }

        /// <summary>
        /// 月完成率
        /// </summary>
        public decimal MonthFinishedPercentage { get; set; }

        /// <summary>
        /// 累计完成率
        /// </summary>
        public decimal CumulativeFinishedPercentage { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
