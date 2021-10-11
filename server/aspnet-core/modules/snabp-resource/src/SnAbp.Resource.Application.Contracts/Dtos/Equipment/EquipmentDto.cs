using SnAbp.Identity;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SnAbp.Basic.Dtos;
using Volo.Abp.Application.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using SnAbp.Utils.TreeHelper;
using SnAbp.Resource.Entities;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentDto : EntityDto<Guid>, IGuidKeyTree<EquipmentDto>
    {
        /// <summary>
        /// 父级设备
        /// </summary>
        public Guid? ParentId { get; set; }
        public EquipmentDto Parent { get; set; }
        public List<EquipmentDto> Children { get; set; }


        /// <summary>
        /// 编号
        /// </summary>
        [MaxLength(50)]
        public string Code { get; set; }


        /// <summary>
        /// CSRG编号
        /// </summary>
        [MaxLength(50)]
        public string CSRGCode { get; set; }


        /// <summary>
        /// 设备名称
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }


        /// <summary>
        /// 构件编码
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public ComponentCategoryDto ComponentCategory { get; set; }


        /// <summary>
        /// 安装位置起点(机房主键)
        /// </summary>
        public Guid? InstallationSiteId { get; set; }
        public InstallationSiteDto InstallationSite { get; set; }


        /// <summary>
        /// 安装位置止点
        /// </summary>
        public Guid? EndInstallationSiteId { get; set; }
        public InstallationSiteDto EndInstallationSite { get; set; }


        /// <summary>
        /// 设备型号编码
        /// </summary>
        public Guid? ProductCategoryId { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }


        /// <summary>
        /// 所属单位id
        /// </summary>
        public Guid? OrganizationId { get; set; }
        public OrganizationDto Organization { get; set; }


        /// <summary>
        /// 维护单位
        /// </summary>
        public List<EquipmentRltOrganizationDto> EquipmentRltOrganizations { get; set; }

        /// <summary>
        /// 厂家编码
        /// </summary>
        public Guid? ManufacturerId { get; set; }
        public ManufacturerDto Manufacturer { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public EquipmentState State { get; set; }


        /// <summary>
        /// 设备类型
        /// </summary>
        public EquipmentType Type { get; set; }


        /// <summary>
        /// 库存设备 Id
        /// </summary>
        public Guid? StoreEquipmentId { get; set; }
        public StoreEquipmentDto StoreEquipment { get; set; }


        [InverseProperty("Equipment")]
        /// <summary>
        /// 库存设备使用记录
        /// </summary>
        public List<EquipmentServiceRecordDto> EquipmentServiceRecords { get; set; }


        /// <summary>
        /// 扩展属性
        /// </summary>
        public List<EquipmentPropertyDto> EquipmentProperties { get; set; }

        /// <summary>
        /// 电缆扩展信息
        /// </summary>
        public Guid? CableExtendId { get; set; }
        public CableExtendDto CableExtend { get; set; }


        [InverseProperty("Equipment")]
        /// <summary>
        /// 端子
        /// </summary>
        public List<TerminalDto> Terminals { get; set; }


        [InverseProperty("Equipment")]
        /// <summary>
        /// 关联文件
        /// </summary>
        public List<EquipmentRltFileDto> EquipmentRltFiles { get; set; }


        /// <summary>
        /// 设备分组（用于3维场景查询使用）通过【设备分组】 + 【设备名称】确定唯一性
        /// </summary>
        public Guid? GroupId { get; set; }
        public EquipmentGroupDto Group { get; set; }


        /// <summary>
        /// 工程量
        /// 
        /// 计算方式：根据改设备的产品分类单位（unit）及改设备的扩展属性进行计算，
        /// 如果单位为“米”、“m”，将属性名称为“长度”、“length”的属性值设为该值。
        /// 
        /// 如：["米","m","M"]->["长度","length","Length"]
        /// 如：["立方米","m3","M3"]->["体积","volume","Volume"]
        /// 如：["平方米","m2","M2"]->["体积","area","Area"]
        /// 
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public List<ComponentRltQRCodeDto> ComponentRltQRCodes { get; set; } = new List<ComponentRltQRCodeDto>();

        /// <summary>
        /// Gis 数据（Json格式），包含相机位置，角度等
        /// </summary>
        public string? GisData { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorName { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

    }
}
