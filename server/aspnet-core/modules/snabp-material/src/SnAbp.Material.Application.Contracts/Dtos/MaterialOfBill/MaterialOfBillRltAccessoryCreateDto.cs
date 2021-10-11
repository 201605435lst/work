using SnAbp.File.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialOfBillRltAccessoryCreateDto
{

        public virtual Guid? MaterialOfBillId { get; set; }

        public virtual Guid? FileId { get; set; }
    }
}
