using System;
using SnAbp.Construction.Enums;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 审批流程 Dto
    /// </summary>
    public class DispatchProcessDto
    {
        /// <summary>
        /// 任务计划id
        /// </summary>
        public Guid DispatchId { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public DispatchState State { get; set; }
    }
}