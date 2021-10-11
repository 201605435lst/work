using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class StationVerySimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }
    }
}
