using SnAbp.File.Dtos;
using SnAbp.Material.Entities;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class PurchaseListRltFileDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid FileId { get; set; }
        public FileSimpleDto File { get; set; }

        /// <summary>
        /// 采购清单
        /// </summary>
        public virtual Guid PurchaseListId { get; set; }
        public virtual PurchaseList PurchaseList { get; set; }
    }
}
