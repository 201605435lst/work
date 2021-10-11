using SnAbp.Resource.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class CableExtendDto : EntityDto<Guid>
    {
        /// <summary>
        /// 芯数
        /// </summary>
        public int? Number { get; set; }


        /// <summary>
        /// 备用芯数
        /// </summary>
        public int? SpareNumber { get; set; }


        /// <summary>
        /// 路产芯数
        /// </summary>
        public int? RailwayNumber { get; set; }


        /// <summary>
        /// 皮长公里
        /// </summary>
        public float? Length { get; set; }


        /// <summary>
        /// 铺设类型
        /// </summary>
        public CableLayType LayType { get; set; }
    }
}
