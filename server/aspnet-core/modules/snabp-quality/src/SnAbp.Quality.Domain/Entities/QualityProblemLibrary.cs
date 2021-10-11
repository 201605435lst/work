/**********************************************************************
*******命名空间： SnAbp.Quality.Entities
*******类 名 称： QualityProblemLibrary
*******类 说 明： 质量问题库
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 18:18:58
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Quality.Entities
{
    /// <summary>
    /// 质量问题库
    /// </summary>
    public class QualityProblemLibrary : Entity<Guid>
    {
        public QualityProblemLibrary(Guid id) => this.Id = id;
        /// <summary>
        /// 问题名称
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public QualityProblemType Type { get; set; }
        /// <summary>
        /// 问题等级
        /// </summary>
        public QualityProblemLevel Level { get; set; }
        /// <summary>
        /// 所属专业，数字字典
        /// </summary>
        public virtual Guid ProfessionId { get; set; }
        public virtual DataDictionary Profession { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        public virtual string Measures { get; set; }

        /// <summary>
        /// 适用范围，为多选的数字字典
        /// </summary>
        public virtual List<QualityProblemLibraryRltScop> Scops { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
