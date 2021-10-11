using SnAbp.Identity;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SnAbp.Basic.Dtos;
using Volo.Abp.Application.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentPropertyUpdateDto : EntityDto<Guid>
    {

        /// <summary>
        /// 所属设备Id
        /// </summary>
        public Guid EquipmentId { get; set; }

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
    }
}
