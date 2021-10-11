/**********************************************************************
*******命名空间： SnAbp.Bpm.Entities
*******类 名 称： WorkflowTemplateGroup
*******类 说 明： 工作流模板分组
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/13 14:09:46
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Bpm.Entities
{
    /// <summary>
    /// 工作流模板分组
    /// </summary>
    public class WorkflowTemplateGroup : Entity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        public void SetId(Guid id) => this.Id = id;
        public virtual string Name { get; set; }//分组名称
        public virtual int Order { get; set; }

        public virtual Guid? ParentId { get; set; }
        [CanBeNull] public virtual WorkflowTemplateGroup Parent { get; set; }

        public virtual List<WorkflowTemplate> WorkflowTemplates { get; set; }
    }
}
