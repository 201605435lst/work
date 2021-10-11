using System.ComponentModel;

namespace SnAbp.Resource.Enums
{
    /// <summary>
    /// 设备运行状态
    /// </summary>
    public enum EquipmentState
    {
        [Description("主用")]
        OnService = 1,

        [Description("封存")]
        OffService = 2,

        [Description("备用")]
        SpareService = 3,

        [Description("报废")]
        Scrap = 4,

        [Description("已安装")]
        Installed = 5
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public enum EquipmentType
    {
        [Description("默认")]
        Default = 1,

        [Description("电缆")]
        Cable = 2,
    }

    /// <summary>
    /// 安装位置类型
    /// </summary>
    public enum InstallationSiteType
    {
        [Description("车站")]
        Station = 1,

        [Description("区间")]
        Section = 2,

        [Description("其他")]
        Other = 3
    }

    /// <summary>
    /// 设备导入标志
    /// </summary>
    public enum EquipmentImportCol
    {
        [Description("序号")]
        SeenSun,
        [Description("车间")]
        Workshop,
        [Description("班组")]
        Team,
        [Description("设备子类")]
        Equip_Sub,
        [Description("单元类型")]
        UnitType,
        [Description("节")]
        Section,
        [Description("设备（设施）编码")]
        CSRGCode,
        [Description("设备名称")]
        Name,
        [Description("系统名称")]
        SystemName,
        [Description("车站-所属线路")]
        Sta_RailwayId,
        [Description("车站-车站名称")]
        Sta_StationId,
        [Description("车站-安装地点")]
        Sta_MachineRoomId,
        [Description("区间-所属区间")]
        Zone_RailwayId,
        [Description("区间-公里标")]
        Zone_KilometerMark,
        [Description("区间-安装地点")]
        Zone_MachineRoomId,
        [Description("其他-所属线路")]
        Oth_RailwayId,
        [Description("其他-安装地点")]
        Oth_MachineRoomId,
        [Description("机房、接入点编码")]
        MachineRoomCode,
        [Description("维护单位编码")]
        MaintenanceOrganizationCode,
        [Description("设备厂家")]
        Manufacture,
        [Description("设备型号")]
        ProductCategory,
        [Description("使用日期")]
        UsedDate,
        [Description("设备运行状态")]
        State,
        [Description("停用日期")]
        UnusedDate,
        [Description("报废日期")]
        ExpiredDate,
        [Description("备注")]
        Remark,
    }

    public enum EquipmentCheckState
    {
        [Description("已审查")]
        Checked = 0,

        [Description("未审查")]
        UnCheck,

        [Description("未通过")]
        Refused
    }
}
