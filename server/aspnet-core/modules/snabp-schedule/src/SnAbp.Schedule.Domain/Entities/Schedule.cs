using SnAbp.Bpm.Entities;
using SnAbp.Identity;
using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Schedule
{
    public class Schedule : SingleFlowEntity, IGuidKeyTree<Schedule>
    {
        public Schedule(Guid id) => Id = id;

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }
        public Schedule Parent { get; set; }
        public List<Schedule> Children { get; set; }

        /// <summary>
        /// 所属专业（数据字典）
        /// </summary>
        public Guid ProfessionId { get; set; }
        public virtual DataDictionary Profession { get; set; }

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
        public List<ScheduleRltSchedule> ScheduleRltSchedules { get; set; }

        /// <summary>
        /// 审核流程（人工审核时为空）
        /// </summary>
        public ScheduleFlowInfo ScheduleFlowInfos { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// 是否为自动审核
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// 工程类型EBS
        /// </summary>
        public List<ScheduleRltProjectItem> ScheduleRltProjectItems { get; set; }

        /// <summary>
        /// 关联设备（模型）
        /// </summary>
        public List<ScheduleRltEquipment> ScheduleRltEquipments { get; set; }
    }
}
