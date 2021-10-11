using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Technology.Enums
{

    public enum InterfaceFlagType
    {
        [Description("接口标记")]
        InterfaceFlag = 1,
        [Description("接口整改")]
        InterfaceFlagReform = 2,
    }
}
