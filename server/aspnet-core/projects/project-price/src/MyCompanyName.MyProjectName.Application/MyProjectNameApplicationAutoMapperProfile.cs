using AutoMapper;
using MyCompanyName.MyProjectName.Dtos;
using MyCompanyName.MyProjectName.Entities;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameApplicationAutoMapperProfile : Profile
    {
        public MyProjectNameApplicationAutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<Module, ModuleDto>();
            CreateMap<ProjectRltModule, ProjectRltModuleDto>();
        }
    }
}
