using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Problem.Dtos
{
    public class ProblemRltProblemCategoryDto : EntityDto<Guid>
    {

        public Guid ProblemId { get; set; }
        public virtual ProblemDto Problem { get; set; }

        public Guid ProblemCategoryId { get; set; }
        public virtual ProblemCategoryDto ProblemCategory { get; set; }
    }
}
