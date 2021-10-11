
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SnAbp.Identity;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowTemplateUpdateMembersDto : EntityDto<Guid>
    {
        [Required]
        /// <summary>
        /// 发布成员
        /// </summary>
        public List<Member> Members { get; set; }
    }
}