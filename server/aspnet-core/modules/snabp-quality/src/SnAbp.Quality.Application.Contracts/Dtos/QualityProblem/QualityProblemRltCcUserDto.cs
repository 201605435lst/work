using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemRltCcUserDto: EntityDto<Guid>
    {
        public Guid QualityProblemId { get; set; }
        public QualityProblemDto QualityProblem { get; set; }
        public virtual Guid CcUserId { get; set; }
        public virtual IdentityUserDto CcUser { get; set; }
    }
}
