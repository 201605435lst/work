/**********************************************************************
*******命名空间： SnAbp.Safe.Dtos.SafeProblem
*******类 名 称： SafeProblemDto
*******类 说 明： 安全问题
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/11 16:34:36
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    /// <summary>
    /// 安全问题
    /// </summary>
    public class SafeProblemDto : EntityDto<Guid>  
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
        public Identity.DataDictionaryDto Type { get; set; }

        /// <summary>
        /// 所属专业，数字字典
        /// </summary>
        public virtual Guid ProfessionId { get; set; }
        public virtual Identity.DataDictionaryDto Profession { get; set; }

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
        /// 最近整改时间
        /// </summary>
        public virtual DateTime? ChangeTime { get; set; }

        /// <summary>
        /// 检查单位
        /// </summary>        
        public virtual Guid? CheckUnitId { get; set; }
        public virtual OrganizationDto CheckUnit { get; set; }

        /// <summary>
        /// 检查单位名称
        /// </summary>        
        public string CheckUnitName { get; set; }

        /// <summary>
        /// 检查人
        /// </summary>
        public virtual Identity.IdentityUser Checker { get; set; }
        public virtual Guid CheckerId { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public virtual Guid? ResponsibleUserId { get; set; }
        public virtual Identity.IdentityUser ResponsibleUser { get; set; }

        /// <summary>
        /// 问题验证人
        /// </summary>
        public virtual Identity.IdentityUser Verifier { get; set; }
        public virtual Guid? VerifierId { get; set; }

        /// <summary>
        /// 责任单位
        /// </summary>
        public virtual string ResponsibleUnit { get; set; }

        /// <summary>
        /// 责任部门
        /// </summary>        
        public virtual Identity.Organization ResponsibleOrganization { get; set; }
        public virtual Guid? ResponsibleOrganizationId { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual SafeProblemState State { get; set; }

        public virtual List<SafeProblemRltCcUserDto> CcUsers { get; set; } = new List<SafeProblemRltCcUserDto>();

        /// <summary>
        /// 问题文件
        /// </summary>
        public virtual List<SafeProblemRltFileDto> Files { get; set; } = new List<SafeProblemRltFileDto>();

        /// <summary>
        /// 关联模型
        /// </summary>
        public virtual List<SafeProblemRltEquipmentDto> Equipments { get; set; } = new List<SafeProblemRltEquipmentDto>();

        /// <summary>
        /// 问题内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 整改措施，整改意见
        /// </summary>
        public virtual string Suggestion { get; set; }
    }
}
