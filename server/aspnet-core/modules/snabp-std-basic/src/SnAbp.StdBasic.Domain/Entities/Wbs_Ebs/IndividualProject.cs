/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Wbs_Ebs
*******类 名 称： IndividualProject
*******类 说 明： 单项工程
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:32:07
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 单项工程
    /// </summary>
    public class IndividualProject : Entity<Guid>, IGuidKeyTree<IndividualProject>
    {
        public IndividualProject(Guid id) => this.Id = id;
        public virtual Guid? ParentId { get; set; }
        public virtual IndividualProject Parent { get; set; }
        public virtual List<IndividualProject> Children { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
       // public virtual string Specialty { get; set; }
        public virtual DataDictionary Specialty { get; set; }
        public virtual Guid SpecialtyId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
