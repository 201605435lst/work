using SnAbp.Identity;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SnAbp.Basic.Dtos;
using Volo.Abp.Application.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentPropertyDto : EntityDto<Guid>
    {
        /// <summary>
        /// 所属设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public EquipmentDto Equipment { get; set; }


        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 属性值
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 关联 MVD 属性分类
        /// </summary>
        public Guid? MVDCategoryId { get; set; }
        public virtual MVDCategoryDto MVDCategory { get; set; }


        /// <summary>
        /// 关联 MVD 属性
        /// </summary>
        public Guid? MVDPropertyId { get; set; }
        public virtual MVDPropertyDto MVDProperty { get; set; }


        public EquipmentPropertyType Type { get; set; }
    }
}
