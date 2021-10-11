
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class ContractRltFileDto : EntityDto<Guid>
    {

        public Guid ContractId { get; set; }
        public ContractDto Contract { get; set; }

        public Guid FileId { get; set; }
        public File.Entities.File File { get; set; }
    }
}
