using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowTemplateChangePublishStateInputDto : EntityDto<Guid>
    {
        [Required]
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }
    }
}