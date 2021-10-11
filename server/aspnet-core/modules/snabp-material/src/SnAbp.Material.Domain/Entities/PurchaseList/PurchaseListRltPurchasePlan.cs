using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    public class PurchaseListRltPurchasePlan : Entity<Guid>
    {
        public PurchaseListRltPurchasePlan(Guid id) => Id = id;
        /// <summary>
        /// 采购计划
        /// </summary>
        public Guid PurchasePlanId { get; set; }
        public PurchasePlan PurchasePlan { get; set; }
        /// <summary>
        /// 采购清单
        /// </summary>
        public Guid PurchaseListId { get; set; }
        public PurchaseList PurchaseList { get; set; }

    }
}
