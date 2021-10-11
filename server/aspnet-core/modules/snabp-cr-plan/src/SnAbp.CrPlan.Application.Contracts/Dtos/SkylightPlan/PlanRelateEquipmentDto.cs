﻿using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.SkylightPlan
{
    public class PlanRelateEquipmentDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 计划编号
        /// </summary>
        public Guid PlanDetailId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public Guid? EquipmentId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

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

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        public PlanRelateEquipmentDto() { }
        public PlanRelateEquipmentDto(Guid id) { Id = id; }
    }
}
