using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class ScheduleUpdateDto : EntityDto<Guid>
    {

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
        public double? TimeLimit { get; set; }

        /// <summary>
        /// 前置任务
        /// </summary>
        public List<ScheduleRltScheduleDto>? ScheduleRltSchedules { get; set; }

        /// <summary>
        /// 审核流程（人工审核时为空）
        /// </summary>
        //public ScheduleFlowInfo ScheduleFlowInfos { get; set; }

        public Guid? WorkflowTemplateId { get; set; }

        /// <summary>
        /// 工程类型EBS
        /// </summary>
        public List<ScheduleRltProjectItem> ScheduleRltProjectItems { get; set; }

        /// <summary>
        /// 是否只更新里程碑
        /// </summary>
        public bool IsMilestone { get; set; } = false;


        /// <summary>
        /// 是否只更新前置任务
        /// </summary>
        public bool IsFrontSchedules { get; set; } = false;


        /// <summary>
        /// 是否是自动审核
        /// </summary>
        public bool IsAuto { get; set; }
    }
}
