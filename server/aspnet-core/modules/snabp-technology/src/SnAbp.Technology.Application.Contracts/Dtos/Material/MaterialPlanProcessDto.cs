/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos.Material
*******类 名 称： MaterialPlanProcessDto
*******类 说 明： 流程审批dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/29/2021 3:59:45 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Technology.enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    ///  用料计划流程处理
    /// </summary>
    public class MaterialPlanProcessDto
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
        public ApprovalStatus Status { get; set; }
    }
}
