using SnAbp.Bpm.Entities;
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
    public class PurchaseListDto : EntityDto<Guid>
    {
        /// <summary>
        /// 采购单编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 采购单名称
        /// </summary>
        public string Name { get; set; }

        public Guid? WorkflowId { get; set; }
        public Guid? WorkflowTemplateId { get; set; }
        /// <summary>
        /// 工作流模板
        /// </summary>
        public virtual WorkflowTemplate WorkflowTemplate { get; set; }
        /// <summary>
        /// 关联项目
        /// </summary>
        public virtual Guid? ProjectId { get; set; }
        public virtual Project.Project Project { get; set; }

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
        /// 采购类型
        /// </summary>
        public PurchaseListType Type { get; set; }

        /// <summary>
        /// 采购清单状态
        /// </summary>
        public PurchaseState State { get; set; }


        /// <summary>
        /// 是否已经提交为采购计划
        /// </summary>
        public bool Submit { get; set; }
        /// <summary>
        /// 关联材料信息
        /// </summary>
        public List<PurchaseListRltFileDto> PurchaseListRltFiles { get; set; } = new List<PurchaseListRltFileDto>();

        /// <summary>
        /// 关联物资信息
        /// </summary>
        public List<PurchaseListRltMaterialDto> PurchaseListRltMaterials { get; set; } = new List<PurchaseListRltMaterialDto>();

        /// <summary>
        /// 采购计划与采购单
        /// </summary>
        public List<PurchaseListRltPurchasePlanDto> PurchaseListRltPurchasePlan { get; set; } = new List<PurchaseListRltPurchasePlanDto>();
    }
}
