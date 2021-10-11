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

            //�������
            CreateMap<Entities.Problem, ProblemDto>();
            CreateMap<ProblemDto, Entities.Problem>();
            CreateMap<Entities.Problem, ProblemCreateDto>();
            CreateMap<Entities.Problem, ProblemUpdateDto>();
            CreateMap<ProblemRltProblemCategory, ProblemRltProblemCategoryDto>();
            CreateMap<ProblemRltProblemCategory, ProblemRltProblemCategoryCreateDto>();

            //�������
            CreateMap<ProblemCategory, ProblemCategoryDto>();
            CreateMap<ProblemCategoryDto, ProblemCategory>();
            CreateMap<ProblemCategory, ProblemCategoryCreateDto>();
            CreateMap<ProblemCategory, ProblemCategoryUpdateDto>();
        }
    }
}