using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Material.Dtos.PurchasePlan
*文件名：PurchasePlanRltMaterialSimpleDto
*创建人： liushengtao
*创建时间：2021/6/28 18:02:11
*描述：
*
***********************************************************************/
namespace SnAbp.Material.Dtos
{
    public class PurchasePlanRltMaterialSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid MaterialId { get; set; }

        /// <summary>
        /// 采购计划
        /// </summary>
        public Guid? PurchasePlanId { get; set; }

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
