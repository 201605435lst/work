using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowTemplateUpdateDto : EntityDto<Guid>
    {
        [Required]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}