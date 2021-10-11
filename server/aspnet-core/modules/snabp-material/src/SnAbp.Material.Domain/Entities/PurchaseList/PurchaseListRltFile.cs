using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    public class PurchaseListRltFile : Entity<Guid>
    {
        public PurchaseListRltFile(Guid id) => Id = id;
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid FileId { get; set; }
        public File.Entities.File File { get; set; }

        /// <summary>
        /// 采购清单
        /// </summary>
        public virtual Guid PurchaseListId { get; set; }
        public virtual PurchaseList PurchaseList { get; set; }
    }
}
