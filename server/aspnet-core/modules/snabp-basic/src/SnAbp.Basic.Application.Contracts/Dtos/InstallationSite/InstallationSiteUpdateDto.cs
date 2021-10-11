using SnAbp.Basic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class InstallationSiteUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 父级
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 安装位置(机房、区域、位置)
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }

        /// <summary>
        /// 使用类别(枚举)
        /// </summary>
        public virtual InstallationSiteUseType? UseType { get; set; }

        /// <summary>
        /// 分类(数据字典)
        /// </summary>
        public virtual Guid? TypeId { get; set; }

        /// <summary>
        /// 类别(数据字典) 中继站、变电所等
        /// </summary>
        public virtual Guid? CategoryId { get; set; }

        /// <summary>
        /// 位置类型(枚举)
        /// </summary>
        public virtual InstallationSiteLocationType? LocationType { get; set; }

        /// <summary>
        /// 线路描述
        /// </summary>
        public virtual RailwayDirection? RailwayDirection { get; set; }

        /// <summary>
        /// 机房位置
        /// </summary>
        [MaxLength(100)]
        public string Location { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 状态(枚举)
        /// </summary>
        public virtual InstallationSiteState State { get; set; }

        /// <summary>
        /// 投产日期
        /// </summary>
        public DateTime? UseDate { get; set; }

        /// <summary>
        /// 线别Id
        /// </summary>
        public virtual Guid? RailwayId { get; set; }

        /// <summary>
        /// 站区Id
        /// </summary>
        public virtual Guid? StationId { get; set; }

        /// <summary>
        /// 起始公里标
        /// </summary>
        public int KMMark { get; set; }
    }
}
