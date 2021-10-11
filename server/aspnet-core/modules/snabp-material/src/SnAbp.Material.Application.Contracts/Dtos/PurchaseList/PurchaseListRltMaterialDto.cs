using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class PurchaseListRltMaterialDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid MaterialId { get; set; }
        public Technology.Dtos.MaterialDto Material { get; set; }

        /// <summary>
        /// 采购清单
        /// </summary>
        public Guid PurchaseListId { get; set; }
        public PurchaseListDto PurchaseList { get; set; }

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
