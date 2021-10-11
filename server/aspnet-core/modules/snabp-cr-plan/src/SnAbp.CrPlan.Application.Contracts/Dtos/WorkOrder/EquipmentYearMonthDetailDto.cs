using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 设备的年表月表工作内容，获取数据使用
    /// 本层为WorkOrderDetailedDto数据第四层
    /// </summary>
    public class EquipmentYearMonthDetailDto : IRepairTagDto
    {
        /// <summary>
        /// 年月表类型
        /// </summary>
        public YearMonthPlanType YearMonthPlanType { get; set; }

        /// <summary>
        /// 工作内容列表
        /// </summary>
        public List<EquipmentPlanDetailDto> PlanDetailedList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
