using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Material.Dtos.PurchasePlan
*文件名：PurchasePlanRltFlowInfoSimpleDto
*创建人： liushengtao
*创建时间：2021/6/28 18:05:13
*描述：
*
***********************************************************************/
namespace SnAbp.Material.Dtos
{
   public class PurchasePlanRltFlowSimpleDto : SingleFlowRltEntity
    {
        public virtual Guid PurchasePlanId { get; set; }
    }
}
