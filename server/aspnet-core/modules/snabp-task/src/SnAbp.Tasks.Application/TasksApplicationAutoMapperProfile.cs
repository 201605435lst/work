using AutoMapper;
using SnAbp.Tasks.Dtos;

namespace SnAbp.Tasks
{
    public class TasksApplicationAutoMapperProfile : Profile
    {
        public TasksApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Task, TaskDto>();
            CreateMap<TaskDto, Task>();
            CreateMap<TaskExtendDto, Task>();
            CreateMap<Task, TaskExtendDto>();
        }
    }
}