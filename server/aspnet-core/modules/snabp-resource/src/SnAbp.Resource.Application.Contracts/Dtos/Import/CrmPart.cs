using SnAbp.Resource.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos
{
    /// <summary>
    /// 产品导入列标记
    /// </summary>
    public enum PartImportCol
    {
        /// <summary>
        /// 序号
        /// </summary>
        SeenSun,
        /// <summary>
        /// 班组
        /// </summary>
        Organization,
        /// <summary>
        /// 设备子类
        /// </summary>
        EquipmentSubclass,
        /// <summary>
        /// 单元类型
        /// </summary>
        ElementType,
        /// <summary>
        /// 组合分类
        /// </summary>
        Group,
        /// <summary>
        /// 节
        /// </summary>
        SectionIFD,
        /// <summary>
        /// 设备编码
        /// </summary>
        CSRGCode,
        /// <summary>
        /// 设备名称
        /// </summary>
        Name,
        /// <summary>
        /// 系统名称
        /// </summary>
        SystemName,

        /// <summary>
        /// 所属线路（车站）
        /// </summary>
        StaRailwayId,
        /// <summary>
        /// 车站名称
        /// </summary>
        StaStationId,
        /// <summary>
        /// 安装地点
        /// </summary>
        StaMachineRoomId,
        /// <summary>
        /// 所属线路（区间）
        /// </summary>
        ZoneRailwayId,
        /// <summary>
        /// 公里标
        /// </summary>
        ZoneKilometerMark,
        /// <summary>
        /// 安装地点
        /// </summary>
        ZoneMachineRoomId,
        /// <summary>
        /// 所属线路(其他）
        /// </summary>
        OthRailwayId,
        /// <summary>
        /// 安装地点
        /// </summary>
        OthMachineRoomId,
        /// <summary>
        /// 机房编码
        /// </summary>
        InstallationSiteCode,

        /// <summary>
        /// 机房编码(安装止点)
        /// </summary>
        EndInstallationSiteCode,

        /// <summary>
        /// 维护单位编码
        /// </summary>
        MaintenanceOrganizationCode,
        /// <summary>
        /// 设备厂家
        /// </summary>
        Manufacturer,
        /// <summary>
        /// 设备型号
        /// </summary>
        ProductCategory,
        /// <summary>
        /// 使用日期
        /// </summary>
        UseDate,
        /// <summary>
        /// 设备运行状态
        /// </summary>
        State,
        /// <summary>
        /// 单位
        /// </summary>
        Unit,
    }

    /// <summary>
    /// 设备分组导入列标记
    /// </summary>
    public class GroupCol {
        public string SeenSun { get; set; }

        public string Name { get; set; }

        public string Order { get; set; }

        public string Organization { get; set; }

        public string OrganizationCode { get; set; }

        public List<PartItem> Items { get; set; }
    }

    /// <summary>
    /// 产品导入数据实体类
    /// </summary>
    public class PartImport
    {
        /// <summary>
        /// excel的sheet名字
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// 对应标准库构件id
        /// </summary>
        public Guid CrmId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string SeenSun { get; set; }

        /// <summary>
        /// 班组
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 设备子类
        /// </summary>
        public string EquipmentSubclass { get; set; }

        /// <summary>
        /// 单元类型
        /// </summary>
        public string ElementType { get; set; }

        /// <summary>
        /// 组合分类
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 节
        /// </summary>
        public string SectionIFD { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string CSRGCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 机房编码
        /// </summary>
        public string InstallationSiteCode { get; set; }
        /// <summary>
        /// 所属线路（车站）
        /// </summary>
        public string StaRailwayId { get; set; }
        /// <summary>
        /// 车站名称
        /// </summary>
        public string StaStationId { get; set; }
        /// <summary>
        /// 安装地点
        /// </summary>
        public string StaMachineRoomId { get; set; }
        /// <summary>
        /// 所属线路（区间）
        /// </summary>
        public string ZoneRailwayId { get; set; }
        /// <summary>
        /// 公里标
        /// </summary>
        public string ZoneKilometerMark { get; set; }
        /// <summary>
        /// 安装地点
        /// </summary>
        public string ZoneMachineRoomId { get; set; }
        /// <summary>
        /// 所属线路(其他）
        /// </summary>
        public string OthRailwayId { get; set; }
        /// <summary>
        /// 安装地点
        /// </summary>
        public string OthMachineRoomId { get; set; }

        public string EndInstallationSiteCode { get; set; }
        /// <summary>
        /// 维护单位编码
        /// </summary>
        public string EquipmentRltOrganizations { get; set; }

        /// <summary>
        /// 设备厂家
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string StandardEquipment { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        public DateTime? UseDate { get; set; }

        /// <summary>
        /// 设备运行状态
        /// </summary>
        public EquipmentState State { get; set; }

        public List<PartItem> Items { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }

    public class PartItem
    {
        public string Name { get; set; }

        public string ParentName { get; set; }
    }

    public class TreeNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级主键
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 详细资料
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<TreeNode> Child { get; set; }
    }
}
