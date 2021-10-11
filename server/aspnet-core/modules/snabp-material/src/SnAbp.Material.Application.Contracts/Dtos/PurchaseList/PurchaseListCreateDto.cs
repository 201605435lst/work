using SnAbp.Identity;
using SnAbp.Material.Enums;
using SnAbp.Project.Dtos;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 采购计划
    /// </summary>
    public class PurchaseListCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 采购单编号
        /// </summary>
        public string Code { get; set; }

        public virtual Guid? WorkflowTemplateId { get; set; }
        /// <summary>
        /// 采购单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 关联项目
        /// </summary>
        public virtual Guid? ProjectId { get; set; }

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
        public string Content { get; set; }

        /// <summary>
        /// 是否已经提交为采购计划
        /// </summary>
        public bool Submit { get; set; }
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
        public List<PurchaseListRltFileCreateDto> PurchaseListRltFiles { get; set; } = new List<PurchaseListRltFileCreateDto>();

        /// <summary>
        /// 关联物资信息
        /// </summary>
        public List<PurchaseListRltMaterialCreateDto> PurchaseListRltMaterials { get; set; } = new List<PurchaseListRltMaterialCreateDto>();
        /// <summary>
        /// 采购计划与采购单
        /// </summary>
        public List<PurchaseListRltPurchasePlanCreateDto> PurchaseListRltPurchasePlan { get; set; } = new List<PurchaseListRltPurchasePlanCreateDto>();
    }
}
