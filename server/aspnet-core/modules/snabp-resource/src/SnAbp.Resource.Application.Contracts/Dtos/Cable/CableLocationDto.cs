using SnAbp.Resource.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class CableLocationDto : EntityDto<Guid>
    {/// <summary>
     /// 电缆 Id
     /// </summary>
        public Guid CableId { get; set; }
        public EquipmentDto Cable { get; set; }


        [MaxLength(100)]
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 参考方向
        /// </summary>
        public CableLocationDirection Direction { get; set; }


        /// <summary>
        /// 参考值
        /// </summary>
        public float Value { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 位置数据（Json String）
        /// </summary>
        public string Positions { get; set; }
    }
}
