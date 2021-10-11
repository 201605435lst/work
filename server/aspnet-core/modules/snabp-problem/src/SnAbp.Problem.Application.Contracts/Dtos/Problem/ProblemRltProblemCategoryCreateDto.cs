using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Problem.Dtos
{
    public class ProblemRltProblemCategoryCreateDto : EntityDto<Guid>
    {
        public Guid ProblemCategoryId { get; set; }
    }
}
