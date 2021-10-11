using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace MyCompanyName.MyProjectName.Dtos
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectUpdateDto : EntityDto<Guid>
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
