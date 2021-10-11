using SnAbp.Construction.Dtos;
using SnAbp.ConstructionBase.Dtos.Section;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联施工区段
    /// </summary>
    public class DispatchRltSectionDto : EntityDto<Guid>
    {
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual DispatchDto Dispatch { get; set; }

        /// <summary>
        /// 施工区段
        /// </summary>
        public virtual Guid SectionId { get; set; }
        public virtual SectionDto Section { get; set; }
    }
}
