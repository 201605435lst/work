using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class ComponentCategoryRltMVDPropertyDto : EntityDto<Guid>
    {
        public Guid? ComponentCategoryId { get; set; }

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

        /// <summary>
        /// 属性Id
        /// </summary>
        public MVDPropertyDto MVDProperty { get; set; }

        public bool IsAll { get; set; }
    }
}
