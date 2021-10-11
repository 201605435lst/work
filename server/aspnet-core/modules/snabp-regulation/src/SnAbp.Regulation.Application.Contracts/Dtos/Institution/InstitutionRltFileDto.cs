using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Institution
{
    public class InstitutionRltFileDto : EntityDto<Guid>
    {

        public Guid FileId { get; set; }

        public File.Dtos.FileSimpleDto File { get; set; }
    }
}
