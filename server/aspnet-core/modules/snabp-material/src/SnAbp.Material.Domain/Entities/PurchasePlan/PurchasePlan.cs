using SnAbp.Bpm.Entities;
using SnAbp.Identity;
using SnAbp.Material.Enums;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 采购计划
    /// </summary>
    public class PurchasePlan : SingleFlowEntity
    {
        public PurchasePlan()
        {

        }
        public void SetId(Guid id) => Id = id;
        public PurchasePlan(Guid id) => Id = id;
        /// <summary>
        /// 计划编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 计划采购时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

        /// <summary>
        /// 是否已经提交为采购计划
        /// </summary>
        public bool Submit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string  Remark{ get; set; }

        /// <summary>
        /// 采购计划状态
        /// </summary>
        public PurchaseState State { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Identity.IdentityUser Creator { get; set; }
        /// <summary>
        /// 关联材料信息
        /// </summary>
        public List<PurchasePlanRltFile> PurchasePlanRltFiles { get; set; }

        /// <summary>
        /// 关联物资信息
        /// </summary>
        public List<PurchasePlanRltMaterial> PurchasePlanRltMaterials { get; set; }

        /// <summary>
        /// 审核流程s
        /// </summary>
        public List<PurchasePlanRltFlow> PurchasePlanRltFlows { get; set; }
    }
}
