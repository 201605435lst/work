using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemRltFileDto : EntityDto<Guid>
    {
        public Guid QualityProblemId { get; set; }
        public QualityProblemDto QualityProblem { get; set; }
        public virtual Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }
    }
}
