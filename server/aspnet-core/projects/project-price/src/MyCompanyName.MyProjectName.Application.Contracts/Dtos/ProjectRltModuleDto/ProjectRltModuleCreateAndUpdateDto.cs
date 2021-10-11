using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace MyCompanyName.MyProjectName.Dtos
{
    public class ProjectRltModuleCreateAndUpdateDto : EntityDto<Guid>, IGuidKeyTree<ProjectRltModuleCreateAndUpdateDto>
    {
        protected ProjectRltModuleCreateAndUpdateDto() { }
        public ProjectRltModuleCreateAndUpdateDto(Guid id) { Id = id; }

        public Guid ProjectId { get; set; }
        public Guid ModuleId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public float Price { get; set; }
        public string Remark { get; set; }
        public Guid? ParentId { get; set; }
        public ProjectRltModuleCreateAndUpdateDto Parent { get; set; }
        public List<ProjectRltModuleCreateAndUpdateDto> Children { get; set; }
    }
}
