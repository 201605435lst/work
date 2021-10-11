using AutoMapper;
using SnAbp.Problem.Dtos;
using SnAbp.Problem.Entities;

namespace SnAbp.Problem
{
    public class ProblemApplicationAutoMapperProfile : Profile
    {
        public ProblemApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            //问题管理
            CreateMap<Entities.Problem, ProblemDto>();
            CreateMap<ProblemDto, Entities.Problem>();
            CreateMap<Entities.Problem, ProblemCreateDto>();
            CreateMap<Entities.Problem, ProblemUpdateDto>();
            CreateMap<ProblemRltProblemCategory, ProblemRltProblemCategoryDto>();
            CreateMap<ProblemRltProblemCategory, ProblemRltProblemCategoryCreateDto>();

            //问题分类
            CreateMap<ProblemCategory, ProblemCategoryDto>();
            CreateMap<ProblemCategoryDto, ProblemCategory>();
            CreateMap<ProblemCategory, ProblemCategoryCreateDto>();
            CreateMap<ProblemCategory, ProblemCategoryUpdateDto>();
        }
    }
}