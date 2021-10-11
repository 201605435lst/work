using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Schedule.Dtos
{
    public class ScheduleCreateDto
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 所属专业（数据字典）
        /// </summary>
        public Guid ProfessionId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 施工部位
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 工作类型（枚举）
        /// </summary>
        public WorkType Type { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 计划工期
        /// </summary>
        public double TimeLimit { get; set; }

        /// <summary>
        /// 前置任务
        /// </summary>
        public List<ScheduleRltScheduleDto> ScheduleRltSchedules { get; set; }

        
        /// <summary>
        /// 审核流程（人工审核时为空）
        /// </summary>
        //public ScheduleFlowInfo ScheduleFlowInfos { get; set; }

        /// <summary>
        /// ScheduleFlowInfos中的工作流模板Id
        /// </summary>
        public Guid? WorkflowTemplateId { get; set; }

        /// <summary>
        /// 工程类型EBS
        /// </summary>
        public List<ScheduleRltProjectItem> ScheduleRltProjectItems { get; set; }

        /// <summary>
        /// 关联设备（模型）
        /// </summary>
        public List<ScheduleRltEquipment> ScheduleRltEquipments { get; set; }

        /// <summary>
        /// 是否是自动审核
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// 批量添加
        /// </summary>
        public List<Guid> ScheduleIdLists { get; set; }
    }
}
