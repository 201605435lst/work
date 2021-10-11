using SnAbp.Resource.Dtos.Equipment_Org;
using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentCreateDto
    {
        /// <summary>
        /// 父级设备
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 构件编码
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }

        ///// <summary>
        ///// 安装位置类型(车站、区间、其它)
        ///// </summary>
        //public InstallationSiteType InstallationSiteType { get; set; }

        /// <summary>
        /// 安装地点(机房主键)
        /// </summary>
        public Guid? InstallationSiteId { get; set; }

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public Guid? ProductCategoryId { get; set; }

        /// <summary>
        /// 所属单位id
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// 维护单位
        /// </summary>
        public List<EquipmentRltOrganizationCreateDto> EquipmentRltOrganizations { get; set; }

        /// <summary>
        /// 厂家编码
        /// </summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public EquipmentState State { get; set; }

        /// <summary>
        /// 设备分组（用于3维场景查询使用）通过【设备分组】 + 【设备名称】确定唯一性
        /// </summary>
        public Guid? GroupId { get; set; }
    }
}
