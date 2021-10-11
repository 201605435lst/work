/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： MaterialPlanRltMaterial
*******类 说 明： 用料计划关联材料表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 11:47:26 AM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Technology.Entities
{
    /// <summary>
    /// 用料计划关联材料     
    /// </summary>
    public class MaterialPlanRltMaterial:Entity<Guid>
    {
        public MaterialPlan MaterialPlan { get; set; }
        public Guid MaterialPlanId { get; set; }
        public Guid MaterialId { get; set; }
        public Material Material { get; set; }
        /// <summary>
        /// 用料数量
        /// </summary>
        public decimal Count { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public MaterialPlanRltMaterial(Guid materialPlanId, Guid materialId)
        {
            this.MaterialPlanId = materialPlanId;
            this.MaterialId = materialId;
        }
        public override object[] GetKeys()
        {
            return new object[] { this.MaterialId, this.MaterialPlanId };
        }
    }
}
