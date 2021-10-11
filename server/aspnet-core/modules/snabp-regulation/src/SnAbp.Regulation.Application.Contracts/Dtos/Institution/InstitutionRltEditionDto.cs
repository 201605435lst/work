using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Institution
{

    public class InstitutionRltEditionDto : EntityDto<Guid>
    {

        public Guid InstitutionId { get; set; }

        public Guid LabelId { get; set; }
    }
}
