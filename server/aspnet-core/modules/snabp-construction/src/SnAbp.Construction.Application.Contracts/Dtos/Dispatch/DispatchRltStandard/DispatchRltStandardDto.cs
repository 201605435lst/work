using SnAbp.ConstructionBase.Dtos.Standard;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联工序指引
    /// </summary>
    public class DispatchRltStandardDto : EntityDto<Guid>
    {
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual DispatchDto Dispatch { get; set; }

        /// <summary>
        /// 工序指引
        /// </summary>
        public virtual Guid StandardId { get; set; }
        public virtual StandardDto Standard { get; set; }
    }
}
