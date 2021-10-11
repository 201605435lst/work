using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemLibraryRltScopSimpleDto : EntityDto<Guid>  
    {
        public Guid SafeProblemLibraryId { get; set; }
        public Guid ScopId { get; set; }
    }
}
