using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
   public class SafeProblemLibraryUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 问题名称
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 事件类型，数字字典
        /// </summary>
        public virtual Guid EventTypeId { get; set; }
        /// <summary>
        /// 所属专业，数字字典
        /// </summary>
        public virtual Guid ProfessionId { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        public SafetyRiskLevel RiskLevel { get; set; }
        /// <summary>
        /// 风险因素
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        public virtual string Measures
        { get; set; }

        /// <summary>
        /// 适用范围，为多选的数字字典
        /// </summary>
        public virtual List<SafeProblemLibraryRltScopSimpleDto> Scops { get; set; } = new List<SafeProblemLibraryRltScopSimpleDto>();
    }
}
