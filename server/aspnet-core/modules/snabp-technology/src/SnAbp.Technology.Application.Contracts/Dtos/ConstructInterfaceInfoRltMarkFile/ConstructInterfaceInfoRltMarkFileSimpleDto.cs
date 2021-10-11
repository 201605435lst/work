using SnAbp.Technology.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
   public  class ConstructInterfaceInfoRltMarkFileSimpleDto : EntityDto<Guid>
    {
        public Guid ConstructInterfaceInfoId { get; set; }
        public Guid MarkFileId { get; set; }
        /// <summary>
        /// 文件类
        /// </summary>
        public InterfaceFlagType Type { get; set; }
    }
}
