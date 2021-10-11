using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Dto.RepairUser
{
    /// <summary>
    /// 设备的测试人员，获取数据使用
    /// 本层为WorkOrderDetailedDto数据第五层
    /// </summary>
    public class RepairUserDto : Entity<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 关联设备表ID
        /// </summary>
        public Guid PlanRelateEquipmentId { get; set; }

        /// <summary>
        /// 作业人员
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 作业人员名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 类型
        /// 0-检修，1-验收
        /// </summary>
        public Duty Duty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        public RepairUserDto() { }

        public RepairUserDto(Guid id) { Id = id; }
    }
}
