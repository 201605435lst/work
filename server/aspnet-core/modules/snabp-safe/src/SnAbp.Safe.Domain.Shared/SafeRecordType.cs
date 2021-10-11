/**********************************************************************
*******命名空间： SnAbp.Safe
*******类 名 称： RecordType
*******类 说 明： 问题记录类型，分为验证、整改
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 19:07:01
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace SnAbp.Safe
{
    /// <summary>
    /// 问题记录类型，分为验证、整改
    /// </summary>
    public enum SafeRecordType
    {
        [Description("整改记录")]
        Improve = 1,
        [Description("验证记录")]
        Verify = 2
    }
}
