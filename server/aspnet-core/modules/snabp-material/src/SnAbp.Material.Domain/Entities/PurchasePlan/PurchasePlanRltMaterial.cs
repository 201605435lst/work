using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    public class PurchasePlanRltMaterial : Entity<Guid>
    {
        public PurchasePlanRltMaterial(Guid id) => Id = id;
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid MaterialId { get; set; }
        public Technology.Entities.Material Material { get; set; }

        /// <summary>
        /// 采购计划
        /// </summary>
        public Guid? PurchasePlanId { get; set; }
        public PurchasePlan PurchasePlan { get; set; }

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
