using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 问题编码-自动生成：事件类型首字母+时间 （待定）
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 问题标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public virtual Guid TypeId { get; set; }

        /// <summary>
        /// 所属专业，数字字典
        /// </summary>
        public virtual Guid ProfessionId { get; set; }

        /// <summary>
        /// 风险等级
        /// </summary>
        public SafetyRiskLevel RiskLevel { get; set; }

        /// <summary>
        /// 检查时间
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// 限制整改时间
        /// </summary>
        public virtual DateTime? LimitTime { get; set; }

        /// <summary>
        /// 检查单位
        /// </summary>        
        public virtual Guid? CheckUnitId { get; set; }

        /// <summary>
        /// 检查单位名称
        /// </summary>        
        public string CheckUnitName { get; set; }

        /// <summary>
        /// 检查人
        /// </summary>
        public virtual Guid CheckerId { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public virtual Guid? ResponsibleUserId { get; set; }

        /// <summary>
        /// 问题验证人
        /// </summary>
        public virtual Guid? VerifierId { get; set; }

        /// <summary>
        /// 责任单位
        /// </summary>
        public virtual string ResponsibleUnit { get; set; }

        /// <summary>
        /// 责任部门
        /// </summary>        
        public virtual Guid? ResponsibleOrganizationId { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual SafeProblemState State { get; set; }

        public virtual List<SafeProblemRltCcUserSimpleDto> CcUsers { get; set; } = new List<SafeProblemRltCcUserSimpleDto>();

        /// <summary>
        /// 问题文件
        /// </summary>
        public virtual List<SafeProblemRltFileSimpleDto> Files { get; set; } = new List<SafeProblemRltFileSimpleDto>();

        /// <summary>
        /// 关联模型
        /// </summary>
        public virtual List<SafeProblemRltEquipmentSimpleDto> Equipments { get; set; } = new List<SafeProblemRltEquipmentSimpleDto>();

        /// <summary>
        /// 风险因素
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 整改措施，整改意见
        /// </summary>
        public virtual string Suggestion { get; set; }
    }
}
