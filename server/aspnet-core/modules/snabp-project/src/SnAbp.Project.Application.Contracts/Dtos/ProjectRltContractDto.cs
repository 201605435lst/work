using SnAbp.Oa.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Project.Dtos
{
    public class ProjectRltContractDto: FullAuditedEntityDto<Guid>
    {
        public ContractDto Contract { get; set; }
        public Guid ContractId { get; set; }
    }
}
