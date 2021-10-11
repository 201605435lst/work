using SnAbp.Construction.Dtos;
using SnAbp.Technology.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单关联材料
    /// </summary>
    public class DispatchRltMaterialDto : EntityDto<Guid>
    {
        /// <summary>
        /// 派工单
        /// </summary>
        public virtual Guid DispatchId { get; set; }
        public virtual DispatchDto Dispatch { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
        public virtual MaterialDto Material { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal Count { get; set; }
    }
}
