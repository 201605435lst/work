/**********************************************************************
*******命名空间： SnAbp.Safe.Entities
*******类 名 称： SafeProblemLibrary
*******类 说 明： 安全问题库
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 18:18:58
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Safe.Entities
{
    /// <summary>
    /// 安全问题库
    /// </summary>
    public class SafeProblemLibrary : Entity<Guid>
    {
        public SafeProblemLibrary(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;

        /// <summary>
        /// 项目id
        /// </summary>
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 事件类型，数字字典
        /// </summary>
        public virtual Guid EventTypeId { get; set; }
        public virtual Identity.DataDictionary EventType { get; set; }
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
        /// 风险因素
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        public virtual string Measures { get; set; }

        /// <summary>
        /// 适用范围，为多选的数字字典
        /// </summary>
        public virtual List<SafeProblemLibraryRltScop> Scops { get; set; }

    }
}
