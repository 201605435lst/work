using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class UpdateGisDataInput : EntityDto<Guid>
    {
        /// <summary>
        /// Gis 数据 Json 字符串
        /// </summary>
        public string GisData { get; set; }
    }
}
