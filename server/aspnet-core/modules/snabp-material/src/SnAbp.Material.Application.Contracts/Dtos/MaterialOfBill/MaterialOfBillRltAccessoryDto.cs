using SnAbp.File.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialOfBillRltAccessoryDto : EntityDto<Guid>
{

        public virtual MaterialOfBillDto MaterialOfBill { get; set; }
        public virtual Guid MaterialOfBillId { get; set; }

        public FileSimpleDto File { get; set; }
        public virtual Guid? FileId { get; set; }
    }
}
