using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Tasks.Entities;
using SnAbp.Tasks.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Tasks
{
    public class Task : FullAuditedEntity<Guid>
    {
        public Task(Guid id) => Id = id;

        /// <summary>
        /// 项目名称
        /// </summary>
        public Guid? ProjectId { get; set; }
        public Project.Project Project { get; set; }

        /// <summary>
        /// 任务主题
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public StateType State { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public PriorityType Priority { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 指派给/负责人
        /// </summary>
        //public Guid? PrincipalId { get; set; }
        //public IdentityUser Principal { get; set; }

        /// <summary>
        /// 抄送给
        /// </summary>
        //public Guid CcPersonId { get; set; }
        //public IdentityUser CcPerson { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 情况描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 子任务父级ID
        /// </summary>
        public Guid? ParentId { get; set; }
        public Task Parent { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public decimal Proportion { get; set; }
        
        /// <summary>
        /// 进度
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 关联抄送成员表
        /// </summary>
        public List<TaskRltMember> TaskRltMembers { get; set; }

        /// <summary>
        /// 关联相关附件表
        /// </summary>
        public List<TaskRltFile> TaskRltFiles { get; set; }
    }
}
