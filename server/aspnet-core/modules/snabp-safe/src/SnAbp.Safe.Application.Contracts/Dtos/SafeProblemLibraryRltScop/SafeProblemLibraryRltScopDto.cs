using SnAbp.Safe.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemLibraryRltScopDto : EntityDto<Guid>  
    {
        public Guid SafeProblemLibraryId { get; set; }
        public SafeProblemLibrary SafeProblemLibrary { get; set; }
        public Guid ScopId { get; set; }
        public virtual Identity.DataDictionary Scop { get; set; }
    }
}
