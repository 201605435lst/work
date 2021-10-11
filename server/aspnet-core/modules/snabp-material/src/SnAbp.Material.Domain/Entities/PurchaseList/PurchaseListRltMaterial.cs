using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    public class PurchaseListRltMaterial : Entity<Guid>
    {
        public PurchaseListRltMaterial(Guid id) => Id = id;
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid MaterialId { get; set; }
        public Technology.Entities.Material Material { get; set; }

        /// <summary>
        /// 采购清单
        /// </summary>
        public Guid PurchaseListId { get; set; }
        public PurchaseList PurchaseList { get; set; }

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
