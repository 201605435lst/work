using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 作业内容内相关设备，获取数据使用
    /// 本层为WorkOrderDetailedDto数据第三层
    /// </summary>
    public class JobContentEquipmentDetailDto : IRepairTagDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public Guid EquipmentId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }


        /// <summary>
        /// 每个设备的年月作业内容列表
        /// </summary>
        public List<EquipmentYearMonthDetailDto> YearMonthDetailedList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
