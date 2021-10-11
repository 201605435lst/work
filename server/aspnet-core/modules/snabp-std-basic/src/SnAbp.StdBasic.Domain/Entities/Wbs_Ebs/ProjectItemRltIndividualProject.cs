/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Wbs_Ebs
*******类 名 称： ProjectItemRltIndividualProject
*******类 说 明： 工程工项与单项工程关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 10:37:27
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
    /// 工程工项与单项工程关联表
    /// </summary>
    public class ProjectItemRltIndividualProject : Entity<Guid>
    {
        public ProjectItemRltIndividualProject(Guid id) => this.Id = id;
        public ProjectItemRltIndividualProject(Guid projectItemId, Guid individualProjectId)
        {
            this.ProjectItemId = projectItemId;
            this.IndividualProjectId = individualProjectId;
        }
        public virtual Guid ProjectItemId { get; set; }
        public virtual Guid IndividualProjectId { get; set; }
        public virtual IndividualProject IndividualProject { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public override object[] GetKeys() => new object[] { ProjectItemId, IndividualProjectId };
    }
}
