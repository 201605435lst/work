using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
   public  class ModelRltMVDPropertyDto: EntityDto<Guid>
    {
        public Guid? ModelId { get; set; }

        //public List<RltMVDPropertyDto> list { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid? MVDPropertyId { get; set; }

        public bool IsAll { get; set; }
    }
}
