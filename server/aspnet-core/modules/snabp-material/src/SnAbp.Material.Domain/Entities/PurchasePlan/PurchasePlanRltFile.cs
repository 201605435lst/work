using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    public class PurchasePlanRltFile : Entity<Guid>
    {
        public PurchasePlanRltFile(Guid id) => Id = id;
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid FileId { get; set; }
        public File.Entities.File File { get; set; }

        /// <summary>
        /// 采购计划
        /// </summary>
        public Guid PurchasePlanId { get; set; }
        public PurchasePlan PurchasePlan { get; set; }
    }
}
