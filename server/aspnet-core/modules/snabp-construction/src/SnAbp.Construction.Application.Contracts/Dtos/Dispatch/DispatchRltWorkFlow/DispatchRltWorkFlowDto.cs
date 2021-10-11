using SnAbp.Bpm.Entities;
using System;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联工作流
    /// </summary>
    public class DispatchRltWorkFlowDto : SingleFlowRltEntity
    {
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual DispatchDto Dispatch { get; set; }
    }
}
