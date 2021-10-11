using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class OrganizationRltLayerCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 图层ids
        /// </summary>
        public List<Guid> LayerIds { get; set; }
    }
}
