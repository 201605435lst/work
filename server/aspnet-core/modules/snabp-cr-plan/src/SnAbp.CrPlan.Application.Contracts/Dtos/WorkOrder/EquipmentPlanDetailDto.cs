using SnAbp.CrPlan.Dto.EquipmentTestResult;
using SnAbp.CrPlan.Dto.RepairUser;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 设备的作业内容实体，获取数据使用
    /// 本层为WorkOrderDetailedDto数据第五层
    /// </summary>
    public class EquipmentPlanDetailDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 计划编号
        /// </summary>
        public Guid PlanDetailId { get; set; }

        /// <summary>
        /// 作业内容
        /// </summary>
        public string WorkContent { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }
        /// <summary>
        /// 作业数量
        /// </summary>
        public decimal WorkCount { get; set; }

        /// <summary>
        /// 是否完成
        /// 0：未做 / 1：合格  /  2：不合格
        /// </summary>
        public AcceptanceResults IsComplete { get; set; }

        /// <summary>
        /// 设备测试项结果
        /// </summary>
        public List<EquipmentTestResultDto> EquipmentTestResultList { get; set; }

        /// <summary>
        /// 检修人
        /// </summary>
        public List<RepairUserDto> MaintenanceUserList { get; set; }


        /// <summary>
        /// 验收人
        /// </summary>
        public List<RepairUserDto> AcceptanceUserList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
