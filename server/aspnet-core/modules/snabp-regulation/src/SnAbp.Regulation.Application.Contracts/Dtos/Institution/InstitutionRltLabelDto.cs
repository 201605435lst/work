using SnAbp.Regulation.Dtos.Label;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Institution
{
    public class InstitutionRltLabelDto : EntityDto<Guid>
    {

        public Guid LabelId { get; set; }

        public LabelDto Label { get; set; }
    }
}
