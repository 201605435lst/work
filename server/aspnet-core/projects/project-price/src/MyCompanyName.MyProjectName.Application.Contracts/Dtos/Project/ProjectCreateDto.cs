using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyCompanyName.MyProjectName.Dtos
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectCreateDto
    {
        public Guid? ParentId { get; set; }


        [Required]
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public float SumPrice { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }


        public List<ProjectRltModuleCreateAndUpdateDto> ProjectRltModules { get; set; } = new List<ProjectRltModuleCreateAndUpdateDto>();
    }
}
