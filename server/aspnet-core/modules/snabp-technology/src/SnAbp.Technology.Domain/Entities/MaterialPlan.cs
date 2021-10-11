/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： MaterialPlen
*******类 说 明： 用料计划表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 11:24:04 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Bpm.Entities;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Technology.enums;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    ///  用料计划表
    /// </summary>
    public class MaterialPlan : SingleFlowEntity
    {
        public void SetId(Guid id) => Id = id;
        public MaterialPlan(Guid id)
        {
            this.Id = id;
        }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Identity.IdentityUser Creator { get; set; }

        /// <summary>
        /// 是否已经提交为采购计划
        /// </summary>
        public bool Submit { get; set; }
        /// <summary>
        /// 审核状态（采购计划的审核状态）
        /// </summary>
        public ApprovalStatus Status { get; set; }
        /// <summary>
        /// 材料信息
        /// </summary>
        public List<MaterialPlanRltMaterial> Materials { get; set; }


        public List<MaterialPlanFlowInfo> MaterialPlanFlowInfos { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
