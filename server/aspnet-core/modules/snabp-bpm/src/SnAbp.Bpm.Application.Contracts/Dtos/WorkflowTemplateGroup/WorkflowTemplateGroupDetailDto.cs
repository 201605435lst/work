/**********************************************************************
*******命名空间： SnAbp.Bpm.Dtos.WorkflowTemplateGroup
*******类 名 称： WorkflowTemplateGroupDetailDto
*******类 说 明： 工作流模板分组dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/13 14:22:00
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    /// <summary>
    /// 工作流模板分组dto
    /// </summary>
    public class WorkflowTemplateGroupDetailDto : EntityDto<Guid>, IGuidKeyTree<WorkflowTemplateGroupDetailDto>, ICodeTree<WorkflowTemplateGroupDetailDto>
    {
        public WorkflowTemplateGroupDetailDto() { }
        public string Name { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
        public WorkflowTemplateGroupDetailDto Parent { get; set; }
        public List<WorkflowTemplateGroupDetailDto> Children { get; set; }
        public string Code { get; set; }
    }
}
