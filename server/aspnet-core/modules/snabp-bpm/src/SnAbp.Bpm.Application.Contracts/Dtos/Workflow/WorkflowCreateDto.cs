using System;
using System.ComponentModel.DataAnnotations;

namespace SnAbp.Bpm.Dtos
{
    public class WorkflowCreateDto
    {
        [Required]
        /// <summary>
        /// 工作流模板 Id
        /// </summary>
        public Guid WorkflowTemplateId { get; set; }


        [Required]
        /// <summary>
        /// 表单值
        /// </summary>
        public string FormValues { get; set; }
    }
}