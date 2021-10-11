/**********************************************************************
*******命名空间： SnAbp.Quality
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
using System.ComponentModel;
using System.Text;

namespace SnAbp.Quality
{
    /// <summary>
    /// 问题记录状态，分为检查中、不通过、已通过
    /// </summary>
    public enum QualityRecordState
    {
        [Description("检查中")]
        Checking = 1,
        [Description("不通过")]
        NotPass = 2,
        [Description("通过")]
        Passed = 3
    }
}
