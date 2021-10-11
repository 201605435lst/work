/**********************************************************************
*******命名空间： SnAbp.Message.MessageDefine
*******类 名 称： MessageContent
*******类 说 明： 消息内容，与数据库表接口几乎同步，只用来记录消息的主要内容，用来匹配模板使用。
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/19 10:23:06
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;

namespace SnAbp.Message.MessageDefine
{
    /// <summary>
    ///     后端模块需要构建的消息内容，为了满足不同需求，新增了Param1-Param10 10个预留参数，参数的使用与具体的消息模板有关系。
    /// </summary>
    [Serializable]
    public abstract class MessageContent
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}