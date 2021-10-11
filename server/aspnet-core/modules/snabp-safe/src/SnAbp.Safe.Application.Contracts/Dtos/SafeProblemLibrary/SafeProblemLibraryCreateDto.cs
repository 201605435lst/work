/**********************************************************************
*******命名空间： SnAbp.Safe.Dtos
*******类 名 称： ProblemLibraryCreateDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/7 15:36:08
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class SafeProblemLibraryCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 工作内容
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
