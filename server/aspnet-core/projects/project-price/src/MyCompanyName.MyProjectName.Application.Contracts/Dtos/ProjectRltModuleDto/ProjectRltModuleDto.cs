using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace MyCompanyName.MyProjectName.Dtos
{
    public class ProjectRltModuleDto : EntityDto<Guid>, IGuidKeyTree<ProjectRltModuleDto>
    {
        protected ProjectRltModuleDto() { }
        public ProjectRltModuleDto(Guid id) { Id = id; }

        public ProjectDto Project { get; set; }
        public Guid ProjectId { get; set; }
        public ModuleDto Module { get; set; }
        public Guid ModuleId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public float Price { get; set; }
        public string Remark { get; set; }
        public Guid? ParentId { get; set; }
        public ProjectRltModuleDto Parent { get; set; }
        public List<ProjectRltModuleDto> Children { get; set; }
    }
}
