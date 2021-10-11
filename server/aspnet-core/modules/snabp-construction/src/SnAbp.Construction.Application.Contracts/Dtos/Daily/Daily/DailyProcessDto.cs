using SnAbp.Construction.Enums;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Construction.Dtos.Daily.Daily
*文件名：Dto
*创建人： liushengtao
*创建时间：2021/7/26 11:33:21
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Dtos
{
    public class DailyProcessDto
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
        public DailyStatus Status { get; set; }
    }
}
