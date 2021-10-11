/**********************************************************************
*******命名空间： SnAbp.Bpm.Dtos.WorkflowTemplateGroup
*******类 名 称： WorkflowTemplateGroupCreateDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/13 14:24:43
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    /// <summary>
    /// 工作流分组创建dto
    /// </summary>
    public class WorkflowTemplateGroupCreateDto : EntityDto<Guid>
    {
        public WorkflowTemplateGroupCreateDto() { }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
