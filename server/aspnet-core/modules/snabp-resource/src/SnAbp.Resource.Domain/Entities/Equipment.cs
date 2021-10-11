using SnAbp.Basic.Entities;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Entities;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    /// 设备信息表
    /// </summary>
    public class Equipment : FullAuditedEntity<Guid>, IGuidKeyTree<Equipment>
    {

        protected Equipment() { }
        public Equipment(Guid id) { Id = id; }

        /// <summary>
        /// 项目id
        /// </summary>
         public Guid? ProjectTagId { get; set; }

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
        /// 标准名称
        /// </summary>
        public string StandardName { get; set; }

        /// <summary>
        /// 父级设备
        /// </summary>
        public Guid? ParentId { get; set; }
        public Equipment Parent { get; set; }
        public List<Equipment> Children { get; set; }

        /// <summary>
        /// 构件分类
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public ComponentCategory ComponentCategory { get; set; }

        /// <summary>
        /// 安装位置起点（默认）
        /// </summary>
        public Guid? InstallationSiteId { get; set; }
        public InstallationSite InstallationSite { get; set; }

        /// <summary>
        /// 安装位置止点
        /// </summary>
        public Guid? EndInstallationSiteId { get; set; }
        public InstallationSite EndInstallationSite { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        public DateTime UseDate { get; set; }

        /// <summary>
        /// 产品分类
        /// </summary>
        public Guid? ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// 所属单位
        /// </summary>
        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }

        /// <summary>
        /// 维护单位
        /// </summary>
        public List<EquipmentRltOrganization> EquipmentRltOrganizations { get; set; }

        /// <summary>
        /// 厂家编码
        /// </summary>
        public Guid? ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

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
        public StoreEquipment StoreEquipment { get; set; }

        [InverseProperty("Equipment")]
        /// <summary>
        /// 库存设备使用记录
        /// </summary>
        public List<EquipmentServiceRecord> EquipmentServiceRecords { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public List<EquipmentProperty> EquipmentProperties { get; set; }

        /// <summary>
        /// 电缆扩展信息
        /// </summary>
        public Guid? CableExtendId { get; set; }
        public CableExtend CableExtend { get; set; }

        [InverseProperty("Equipment")]
        /// <summary>
        /// 端子
        /// </summary>
        public List<Terminal> Terminals { get; set; }

        [InverseProperty("Equipment")]
        /// <summary>
        /// 关联文件
        /// </summary>
        public List<EquipmentRltFile> EquipmentRltFiles { get; set; }

        /// <summary>
        /// 设备分组（用于3维场景查询使用）通过【设备分组】 + 【设备名称】确定唯一性
        /// </summary>
        public Guid? GroupId { get; set; }
        public EquipmentGroup Group { get; set; }

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
        /// Gis 数据（Json格式），包含相机位置，角度等
        /// </summary>
        public string? GisData { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>

        [InverseProperty("GenerateEquipment")]
        public List<ComponentRltQRCode> ComponentRltQRCodes { get; set; }

        public EquipmentCheckState CheckState { get; set; }
    }
}
