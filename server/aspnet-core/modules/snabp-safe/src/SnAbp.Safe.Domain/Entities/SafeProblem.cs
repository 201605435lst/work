/**********************************************************************
*******命名空间： SnAbp.Safe.Entities
*******类 名 称： SafeProblem
*******类 说 明： 安全问题
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 18:32:16
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Safe.Entities
{
    /// <summary>
    /// 安全问题
    /// </summary>
    public class SafeProblem : AuditedEntity<Guid>
    {
        public SafeProblem(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;

        /// <summary>
        /// 项目id
        /// </summary>
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
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
        public Identity.DataDictionary Type { get; set; }
        /// <summary>
        /// 所属专业，数字字典
        /// </summary>
        public virtual Guid ProfessionId { get; set; }
        public virtual Identity.DataDictionary Profession { get; set; }
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
        public virtual Organization CheckUnit { get; set; }
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

        public virtual List<SafeProblemRltCcUser> CcUsers { get; set; }
        /// <summary>
        /// 问题文件
        /// </summary>
        public virtual List<SafeProblemRltFile> Files { get; set; }
        /// <summary>
        /// 关联模型
        /// </summary>
        public virtual List<SafeProblemRltEquipment> Equipments { get; set; }
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
