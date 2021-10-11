/**********************************************************************
*******命名空间： SnAbp.Message.MessageDefine
*******类 名 称： MessageType
*******类 说 明： 消息类型枚举定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/16 15:13:19
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Message.MessageDefine
{
    /// <summary>
    /// 消息体类型，设计到消息模板的选择
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 默认消息
        /// </summary>
        Default,
        /// <summary>
        /// 通知信息
        /// </summary>
        Notice
    }

    /// <summary>
    /// 发送模式类型
    /// </summary>
    public enum SendModeType
    {
        /// <summary>
        /// 发送给指定的用户
        /// </summary>
        User,
        /// <summary>
        /// 发送给指定的组织结构
        /// </summary>
        Organization,
        /// <summary>
        /// 发送给指定的角色
        /// </summary>
        Role,
        /// <summary>
        /// 发送给指定的成员信息
        /// </summary>
        Member,
        /// <summary>
        /// 默认发送，发送给所有连接的客户端
        /// </summary>
        Default
    }

    /// <summary>
    /// 客户端发送给服务器端的消息类型
    /// </summary>
    public enum ClientSendType
    {
        /// <summary>
        /// 确认
        /// </summary>
        Confirm,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 已读
        /// </summary>
        Read
    }

    /// <summary>
    /// 客户端接收的消息类型，在发送方法中获取
    /// </summary>
    public enum ClientReceiveType
    {
        /// <summary>
        /// 初始化,客户端注册完成后接收的类型，整个生命周期只使用一次
        /// </summary>
        Init,
        /// <summary>
        /// 实时接收
        /// </summary>
        Real
    }
}
