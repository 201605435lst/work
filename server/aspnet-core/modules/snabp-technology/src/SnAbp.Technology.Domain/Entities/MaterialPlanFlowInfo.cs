/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： MaterialPlanFlowInfo
*******类 说 明： 用料计划流程信息
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:31:19 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Bpm.Entities;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    ///  用料计划流程信息
    /// </summary>
    public class MaterialPlanFlowInfo: SingleFlowRltEntity
    {
        public MaterialPlanFlowInfo(Guid id) => Id = id;

        public Guid MaterialPlanId { get; set; }
        public MaterialPlan MaterialPlan { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
