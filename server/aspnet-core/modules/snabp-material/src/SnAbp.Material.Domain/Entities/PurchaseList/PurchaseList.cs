using SnAbp.Bpm.Entities;
using SnAbp.Identity;
using SnAbp.Material.Enums;
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 采购单
    /// </summary>
    public class PurchaseList : SingleFlowEntity
    {
        public PurchaseList()
        {

        }
        public void SetId(Guid id) => Id = id;
        public PurchaseList(Guid id) => Id = id;
        /// <summary>
        /// 采购单编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 采购单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否已经提交为采购计划
        /// </summary>
        public bool Submit { get; set; }

        /// <summary>
        /// 计划采购时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

        /// <summary>
        /// 登记人员
        /// </summary>
        public Identity.IdentityUser Creator { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string  Content{ get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        public PurchaseListType Type { get; set; }

        /// <summary>
        /// 采购清单状态
        /// </summary>
        public PurchaseState State { get; set; }

        /// <summary>
        /// 关联材料信息
        /// </summary>
        public List<PurchaseListRltFile> PurchaseListRltFiles { get; set; }

        /// <summary>
        /// 关联物资信息
        /// </summary>
        public List<PurchaseListRltMaterial> PurchaseListRltMaterials { get; set; }

        /// <summary>
        /// 审核流程
        /// </summary>
        public List<PurchaseListRltFlow> PurchaseListRltFlows { get; set; }
        /// <summary>
        /// 采购计划与采购单
        /// </summary>
        public List<PurchaseListRltPurchasePlan> PurchaseListRltPurchasePlan { get; set; }
    }
}
