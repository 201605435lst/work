using System;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联人员
    /// </summary>
    public class DispatchRltWorkerCreateDto
    {
        /// <summary>
        /// 人员
        /// </summary>
        public virtual Guid WorkerId { get; set; }
    }
}
