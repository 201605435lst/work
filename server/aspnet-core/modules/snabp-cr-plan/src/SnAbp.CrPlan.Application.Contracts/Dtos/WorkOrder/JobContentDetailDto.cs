using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 作业内容实体，获取数据使用
    /// 本层为WorkOrderDetailedDto数据第二层
    /// </summary>
    public class JobContentDetailDto : IRepairTagDto
    {
        /// <summary>
        /// 设备名称/类别
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备名称/类别序号
        /// </summary>
        public int DeviceNumber { get; set; }

        /// <summary>
        /// 相关设备列表
        /// </summary>
        public List<JobContentEquipmentDetailDto> JobContentEquipmentList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
