using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
   public class ConstructionSectionCreateDto : EntityDto<Guid>

    {
        /// <summary>
        /// 施工区段名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 起始锚段
        /// </summary>
        public string StartSegment { get; set; }
        /// <summary>
        /// 终止锚段
        /// </summary>
        public string EndSegment { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
