/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos.ConstructInterfaceInfo
*******类 名 称： Class1
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:50:55 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Technology.enums;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    ///  用料计划dto
    /// </summary>
    public class MaterialPlanDto : EntityDto<Guid>
    {
        public string PlanName { get; set; }
        public Guid WorkflowId { get; set; }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string Code { get; set; }
        public DateTime PlanTime { get; set; }
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public ApprovalStatus Status { get; set; }
        /// <summary>
        /// 是否提交了采购计划生成
        /// </summary>
        public bool Submit { get; set; }
        public Identity.IdentityUserDto Creator { get; set; }
        public List<MaterialPlanRltMaterialDto> Materials { get; set; }
    }

}