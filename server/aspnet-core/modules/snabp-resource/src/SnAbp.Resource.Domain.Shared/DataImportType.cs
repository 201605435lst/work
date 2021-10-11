/**********************************************************************
*******命名空间： SnAbp.Resource
*******类 名 称： DataImportType
*******类 说 明： 数据读入类型枚举
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 11:55:48
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource
{
    public enum DataImportType
    {
        /// <summary>
        /// 工程设备
        /// </summary>
        EngineEquipment,
        /// <summary>
        /// 工程电缆
        /// </summary>
        EngineCable,
        /// <summary>
        /// 电缆配线
        /// </summary>
        CableWiring,
        /// <summary>
        /// 机柜配线
        /// </summary>
        CabinetWiring,
    }
}
