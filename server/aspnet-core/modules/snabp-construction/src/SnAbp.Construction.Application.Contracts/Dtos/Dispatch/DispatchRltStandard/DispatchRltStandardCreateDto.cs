using System;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联工序指引
    /// </summary>
    public class DispatchRltStandardCreateDto
    {
        /// <summary>
        /// 工序指引
        /// </summary>
        public virtual Guid StandardId { get; set; }
    }
}
