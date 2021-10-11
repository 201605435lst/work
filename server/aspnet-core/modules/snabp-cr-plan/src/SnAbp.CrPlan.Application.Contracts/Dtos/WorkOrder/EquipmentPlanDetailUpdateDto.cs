using SnAbp.CrPlan.Dto.EquipmentTestResult;
using SnAbp.CrPlan.Dto.RepairUser;
using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 工作内容、设备测试项、检修人员修改实体
    /// </summary>
    public class EquipmentPlanDetailUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 作业数量
        /// </summary>
        public decimal WorkCount { get; set; }


        /// <summary>
        /// 设备测试项结果
        /// </summary>
        public List<EquipmentTestResultUpdateDto> EquipmentTestResultList { get; set; }

        /// <summary>
        /// 检修人
        /// </summary>
        public List<Guid> MaintenanceUserList { get; set; }


        /// <summary>
        /// 验收人
        /// </summary>
        public List<Guid> AcceptanceUserList { get; set; }
        public string RepairTagKey { get ; set ; }
    }
}
