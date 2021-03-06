using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
     public class RltMVDPropertyDto {
        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid? MVDPropertyId { get; set; }
    }

   public class ModelRltMVDPropertyCreateDto : PagedAndSortedResultRequestDto
    {
        public Guid ModelId { get; set; }

        public List<RltMVDPropertyDto> list { get; set; }

        public bool IsAll { get; set; }
    }
}
