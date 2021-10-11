﻿/**********************************************************************
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
using System.ComponentModel;

namespace SnAbp.Quality
{
    /// <summary>
    /// 消息发送类型
    /// </summary>
    public enum QualityProblemLevel
    {
        [Description("重大质量事故")]
        Great = 1,
        [Description("一般质量事故")]
        General = 2,
        [Description("质量问题")]
        Minor = 3
    }
}
