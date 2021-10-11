using SnAbp.Bpm.Entities;
using SnAbp.MultiProject.MultiProject;
using System;

namespace SnAbp.Construction.Entities
{
    /// <summary>
    /// 派工单关联人员
    /// </summary>
    public class DispatchRltWorkFlow : SingleFlowRltEntity
    {
        public DispatchRltWorkFlow(Guid id) => Id = id;
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual Dispatch Dispatch { get; set; }
    }
}
