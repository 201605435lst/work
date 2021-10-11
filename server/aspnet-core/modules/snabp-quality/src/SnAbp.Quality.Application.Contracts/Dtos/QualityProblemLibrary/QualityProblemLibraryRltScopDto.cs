using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemLibraryRltScopDto : EntityDto<Guid>
    {
        public Guid QualityProblemLibraryId { get; set; }
        public QualityProblemLibraryDto QualityProblemLibrary { get; set; }
        public Guid ScopId { get; set; }
        public virtual DataDictionaryDto Scop { get; set; }
    }
}
