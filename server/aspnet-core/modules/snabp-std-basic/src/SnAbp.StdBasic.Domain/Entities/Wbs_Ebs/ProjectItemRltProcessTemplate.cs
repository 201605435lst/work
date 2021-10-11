/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Wbs_Ebs
*******类 名 称： ProjectItemRltProcessTemplate
*******类 说 明： 工序模板与工程关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:42:21
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 工序模板与工程关联表
    /// </summary>
    public class ProjectItemRltProcessTemplate : Entity<Guid>
    {
        public ProjectItemRltProcessTemplate(Guid id) => this.Id = id;
        public ProjectItemRltProcessTemplate(Guid projectItemId, Guid processTemplateId)
        {
            this.ProjectItemId = projectItemId;
            this.ProcessTemplateId = processTemplateId;
        }
        public virtual Guid ProjectItemId { get; set; }
        public virtual Guid ProcessTemplateId { get; set; }
        public virtual ProcessTemplate ProcessTemplate { get; set; }
        //public virtual ProjectItem ProjectItem { get; set; }
        public override object[] GetKeys() => new object[] { ProjectItemId, ProcessTemplateId };
    }
}
