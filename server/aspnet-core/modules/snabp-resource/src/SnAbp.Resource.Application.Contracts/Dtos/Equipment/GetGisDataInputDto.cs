using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class GetGisDataInputDto : EntityDto<Guid>
    {
        /// <summary>
        /// 设备分组
        /// </summary>
        public string GroupName { get; set; }


        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
    }
}
