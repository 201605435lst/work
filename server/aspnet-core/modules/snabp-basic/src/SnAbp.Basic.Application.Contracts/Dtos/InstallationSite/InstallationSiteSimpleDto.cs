using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class InstallationSiteSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 机房名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子项
        /// </summary>
        public List<InstallationSiteSimpleDto> Children { get; set; }
    }
}
