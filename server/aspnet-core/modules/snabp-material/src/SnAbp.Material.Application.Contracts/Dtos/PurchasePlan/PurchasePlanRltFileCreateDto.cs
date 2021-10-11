using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class PurchasePlanRltFileCreateDto
    {
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// 采购计划
        /// </summary>
        public Guid PurchasePlanId { get; set; }
    }
}
