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
    public class PurchasePlanUpdateDto : EntityDto<Guid>
    {


        /// <summary>
        /// 计划名称
        /// </summary>
        public string Name { get; set; }

        public virtual Guid? WorkflowTemplateId { get; set; }

        /// <summary>
        /// 关联项目
        /// </summary>
        public virtual Guid? ProjectId { get; set; }

        /// <summary>
        /// 计划采购时间
        /// </summary>
        public DateTime? PlanTime { get; set; }



        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否已经提交为采购计划
        /// </summary>
        public bool Submit { get; set; }
        /// <summary>
        /// 采购计划状态
        /// </summary>
        public PurchaseState State { get; set; }

        /// <summary>
        /// 关联资料信息
        /// </summary>
        public List<PurchasePlanRltFileSimpleDto> PurchasePlanRltFiles { get; set; } = new List<PurchasePlanRltFileSimpleDto>();

        /// <summary>
        /// 关联物资信息
        /// </summary>
        public List<PurchasePlanRltMaterialSimpleDto> PurchasePlanRltMaterials { get; set; } = new List<PurchasePlanRltMaterialSimpleDto>();

      
    }
}
