using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Material.Dtos.PurchaseList
*文件名：PurchaseListProcessDto
*创建人： liushengtao
*创建时间：2021/6/30 17:49:29
*描述：
*
***********************************************************************/
namespace SnAbp.Material.Dtos
{
   public class PurchaseListProcessDto
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
