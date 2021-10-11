using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class PurchasePlanRltMaterialDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid MaterialId { get; set; }
        public Technology.Dtos.MaterialDto Material { get; set; }

        /// <summary>
        /// 采购计划
        /// </summary>
        public Guid PurchasePlanId { get; set; }
        public PurchasePlanDto PurchasePlan { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 合同价
        /// </summary>
        public double Price { get; set; }

    }
}
