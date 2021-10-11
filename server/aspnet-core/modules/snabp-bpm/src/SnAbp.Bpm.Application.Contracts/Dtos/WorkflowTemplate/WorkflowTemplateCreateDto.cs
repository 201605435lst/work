using System;
using System.ComponentModel.DataAnnotations;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowTemplateCreateDto
    {
        [Required]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组id
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 模板类型
        /// </summary>
        public WorkflowTemplateType Type { get; set; }
    }
}