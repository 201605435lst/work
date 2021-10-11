using System;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联施工区段
    /// </summary>
    public class DispatchRltSectionCreateDto
    {
        /// <summary>
        /// 施工区段
        /// </summary>
        public virtual Guid SectionId { get; set; }
    }
}
