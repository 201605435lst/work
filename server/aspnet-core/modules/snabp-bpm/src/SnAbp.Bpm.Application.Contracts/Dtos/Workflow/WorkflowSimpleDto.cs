using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowSimpleDto : Entity<Guid>
    {
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 简报
        /// </summary>
        public List<WorkflowInfo> Infos { get; set; }


        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime LastModificationTime { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public WorkflowState State { get; set; }


        /// <summary>
        /// 表单版本
        /// </summary>
        public int FormTemplateVersion { get; set; }


        /// <summary>
        /// 流程版本
        /// </summary>
        public int FlowTemplateVersion { get; set; }

        public bool IsStatic { get; set; }
      
    }
}