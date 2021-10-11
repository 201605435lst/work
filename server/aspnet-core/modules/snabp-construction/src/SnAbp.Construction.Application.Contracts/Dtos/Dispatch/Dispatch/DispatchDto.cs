using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using SnAbp.Construction.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单
    /// </summary>
    public class DispatchDto : EntityDto<Guid>
    {
        //提交人
        public Guid? CreatorId { get; set; }
        public IdentityUserDto Creator { get; set; }

        /// <summary>
        /// 工作流模板
        /// </summary>
        public Guid? WorkflowId { get; set; }
        public Guid? WorkflowTemplateId { get; set; }
        public virtual WorkflowTemplate WorkflowTemplate { get; set; }

        /// <summary>
        /// 派工单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 派工编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 派工单模板
        /// </summary>
        public virtual Guid? DispatchTemplateId { get; set; }
        public virtual DispatchTemplateDto DispatchTemplate { get; set; }

        /// <summary>
        /// 施工专业
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// 施工任务
        /// </summary>
        public List<DispatchRltPlanContentDto> DispatchRltPlanContents { get; set; }

        /// <summary>
        /// 施工区段
        /// </summary>
        public List<DispatchRltSectionDto> DispatchRltSections { get; set; }

        /// <summary>
        /// 工序指引
        /// </summary>
        public List<DispatchRltStandardDto> DispatchRltStandards { get; set; }

        /// <summary>
        /// 施工材料
        /// </summary>
        public List<DispatchRltMaterialDto> DispatchRltMaterials { get; set; }

        /// <summary>
        /// 承包商
        /// </summary>
        public virtual Guid ContractorId { get; set; }
        public virtual DataDictionary Contractor { get; set; }

        /// <summary>
        /// 施工班组
        /// </summary>
        public string Team { get; set; }

        /// <summary>
        /// 施工员
        /// </summary>
        public List<DispatchRltWorkerDto> DispatchRltWorkers { get; set; }

        /// <summary>
        /// 施工人员数量
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 派工单日期
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 补充说明
        /// </summary>
        public string ExtraDescription { get; set; }

        /// <summary>
        /// 是否需要大型吊装设备
        /// </summary>
        public bool IsNeedLargeEquipment { get; set; }

        /// <summary>
        /// 大型吊装设备
        /// </summary>
        public string LargeEquipment { get; set; }

        /// <summary>
        /// 是否涉及围蔽拆除
        /// </summary>
        public bool IsDismantle { get; set; }

        /// <summary>
        /// 是否涉及高空作业
        /// </summary>
        public bool IsHighWork { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 安全风险源
        /// </summary>
        public string RiskSources { get; set; }

        /// <summary>
        /// 计划恢复时间
        /// </summary>
        public DateTime? RecoveryTime { get; set; }

        /// <summary>
        /// 安全防护措施
        /// </summary>
        public string SafetyMeasure { get; set; }

        /// <summary>
        /// 工序控制类型
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// 关联资料
        /// </summary>
        public List<DispatchRltFileDto> DispatchRltFiles { get; set; }

        /// <summary>
        /// 其他事宜（备注）
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 审批流程状态 默认未提交 
        /// </summary>
        public DispatchState State { get; set; }

        /// <summary>
        /// 审批流程节点
        /// </summary>
        public List<CommentDto> WorkFlowNodes { get; set; }

    }
}
