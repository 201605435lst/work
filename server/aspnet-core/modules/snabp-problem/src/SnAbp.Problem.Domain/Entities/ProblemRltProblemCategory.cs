using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Problem.Entities
{
    public class ProblemRltProblemCategory : Entity<Guid>
    {
        public Guid ProblemId { get; set; }
        public virtual Problem Problem { get; set; }

        public Guid ProblemCategoryId { get; set; }
        public virtual ProblemCategory ProblemCategory { get; set; }

        protected ProblemRltProblemCategory() { }
        public ProblemRltProblemCategory(Guid id) { Id = id; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
