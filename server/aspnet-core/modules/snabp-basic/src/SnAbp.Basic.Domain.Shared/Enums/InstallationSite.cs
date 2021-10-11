using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Basic.Enums
{
    /// <summary>
    /// 机房状态
    /// </summary>
    public enum InstallationSiteState
    {
        [Description("在用")]
        Using = 1,
        [Description("在建")]
        Building = 2,
    }

    /// <summary>
    /// 位置类型
    /// </summary>
    public enum InstallationSiteLocationType
    {
        [Description("非沿线")]
        RailwayOuter = 1,
        [Description("沿线区间")]
        SectionInner = 2,
        [Description("沿线站内")]
        StationInner = 3,
        [Description("其他")] 
        Other = 4,

    }


    /// <summary>
    /// 安装位置使用类型
    /// </summary>
    public enum InstallationSiteUseType
    {
        [Description("独用")]
        Private = 1,
        [Description("共用")]
        Share = 2,
    }


    /// <summary>
    /// 导入列标志
    /// </summary>
    public enum ImportCol
    {
        [Description("序号")]
        SeenSun,
        [Description("编码")]
        CSRGCode,
        [Description("机房名称")]
        Name,
        [Description("合用单位")]
        Organization,
        [Description("使用类别")]
        UseType,
        [Description("机房类别")]
        Type,
        [Description("机房分类")]
        Category,
        [Description("位置分类")]
        LocationType,
        [Description("线路")]
        Railway,
        [Description("机房位置")]
        Location,
        [Description("经度")]
        Longitude,
        [Description("纬度")]
        Latitude,
        [Description("状态")]
        State,
        [Description("投产日期")]
        UseDate

    }
}
