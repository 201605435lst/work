using SnAbp.Material.Entities;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialAcceptanceRltPurchaseDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联验收单
        /// </summary>
        public virtual Guid MaterialAcceptanceId { get; set; }
        public virtual MaterialAcceptanceDto MaterialAcceptance { get; set; }

        /// <summary>
        /// 采购清单
        /// </summary>
        public virtual Guid PurchaseListId { get; set; }
        public virtual PurchaseList PurchaseList { get; set; }
    }
}
