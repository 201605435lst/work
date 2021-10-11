using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Material.Dtos.PurchasePlan
*文件名：PurchasePlanProcessDto
*创建人： liushengtao
*创建时间：2021/6/30 16:45:14
*描述：采购计划流程处理
*
***********************************************************************/
namespace SnAbp.Material.Dtos
{
    public class PurchasePlanProcessDto
    {
        /// <summary>
        /// 计划id
        /// </summary>
        public Guid PlanId { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 审批状态（pass 和unpass)
        /// </summary>
        public PurchaseState State { get; set; }
    }
}
