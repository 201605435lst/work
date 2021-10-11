using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

/************************************************************************************
*命名空间：SnAbp.Material.Dtos.PurchasePlan
*文件名：PurchasePlanRltFileSimpleDto
*创建人： liushengtao
*创建时间：2021/6/28 18:00:31
*描述：
*
***********************************************************************/
namespace SnAbp.Material.Dtos
{
   public  class PurchasePlanRltFileSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联材料
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// 采购清单
        /// </summary>
        public Guid PurchaseListId { get; set; }

    }
}
