using SnAbp.Identity;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class EquipmentOfComponentTrackDto : EntityDto<Guid>
    {
        /// <summary>
        /// 父级设备
        /// </summary>
        public Guid? ParentId { get; set; }
        public EquipmentDto Parent { get; set; }


        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// CSRG编号
        /// </summary>
        public string CSRGCode { get; set; }


        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 构件编码
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public ComponentCategoryDto ComponentCategory { get; set; }


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


        /// <summary>
        /// 端子
        /// </summary>
        public List<TerminalDto> Terminals { get; set; }


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
        /// Gis 数据（Json格式），包含相机位置，角度等
        /// </summary>
        public string? GisData { get; set; }
    }
}