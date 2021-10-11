using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class PurchasePlanRltMaterialCreateDto
    {
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid MaterialId { get; set; }
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
