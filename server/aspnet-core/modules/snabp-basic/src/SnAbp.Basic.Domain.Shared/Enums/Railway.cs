using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Basic.Enums
{
    public enum RailwayType
    {
        [Description("单线")]
        MONGLINE = 0,
        [Description("复线")]
        COMPLEXLINE = 1
    }

    public enum RailwayDirection
    {
        [Description("上行")]
        Up = 0,
        [Description("下行")]
        Down = 1,
        [Description("上下行")]
        UpAndDown = 2
    }

    public enum RailwayInportCol
    {
        [Description("SeenSun")]
        SeenSun,
        [Description("Organization")]
        Organization,
        [Description("Name")]
        Name,
        [Description("Type")]
        Type,
        [Description("DownLink")]
        DownLink,
        [Description("UpLink")]
        UpLink,
    }
}
