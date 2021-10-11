/**********************************************************************
*******命名空间： SnAbp.Message.MessageDefine
*******类 名 称： BaseMessage
*******类 说 明： 基础消息数据封装
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/16 14:51:18
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SnAbp.Message.MessageDefine
{
    [Serializable]
    public abstract class BaseMessage : IBaseMessage
    {
        /// <summary>
        ///     消息主题
        /// </summary>
        public virtual string Topic { get; set; }

        /// <summary>
        ///     消息体,消息内容，可根据<see cref="MessageContent" />查看具体的消息内容
        /// </summary>
        public virtual MessageContent Content { get; set; }

        /// <summary>
        /// 向客户端发送的数据
        /// </summary>
        public virtual object SendData { get; set; }

        /// <summary>
        ///     消息类型
        /// </summary>
        public virtual MessageType Type { get; set; }

        /// <summary>
        ///     消息发送类型,默认发送模式是所有连接的客户端<see cref="SendModeType.Default" />
        /// </summary>
        public virtual SendModeType SendType { get; set; } = SendModeType.Default;

        /// <summary>
        ///     私有用户id，用于指定用户发送
        /// </summary>
        protected List<Guid> UserIds { get; set; }

        /// <summary>
        ///     需要接受消息的组织结构id
        /// </summary>
        protected List<Guid> OrganizationIds { get; set; }

        /// <summary>
        ///     需要接受消息的组织结构id
        /// </summary>
        protected List<Guid> RoleIds { get; set; }

        /// <summary>
        ///     添加需要接受消息的用户id
        /// </summary>
        /// <param name="id"></param>
        public void SetUserId(Guid id) => (UserIds ??= new List<Guid>()).Add(id);

        public void SetUserIds(Guid[] ids) => (UserIds ??= new List<Guid>()).AddRange(ids);
        public void SetRoleId(Guid id) => (RoleIds ??= new List<Guid>()).Add(id);
        public void SetRoleIds(Guid[] ids) => (RoleIds ??= new List<Guid>()).AddRange(ids);

        public void SetMembers(List<Member> members)
        {
            (UserIds ??= new List<Guid>()).AddRange(members.Where(a => a.Type == MemberType.User).Select(a => a.Id));
            (OrganizationIds ??= new List<Guid>()).AddRange(members.Where(a => a.Type == MemberType.Organization)
                .Select(a => a.Id));
            (RoleIds ??= new List<Guid>()).AddRange(members.Where(a => a.Type == MemberType.Role).Select(a => a.Id));
        }

        public void SetOrganizationId(Guid id) => (OrganizationIds ??= new List<Guid>()).Add(id);
        public void SetOrganizationIds(Guid[] ids) => (OrganizationIds ??= new List<Guid>()).AddRange(ids);

        public IReadOnlyList<Member> GetMembers() => new List<Member>()
            .AddNewRange(UserIds?.Select(a => new Member {Id = a, Type = MemberType.User}))
            .AddNewRange(RoleIds?.Select(a => new Member {Id = a, Type = MemberType.Role}))
            .AddNewRange(OrganizationIds?.Select(a => new Member {Id = a, Type = MemberType.Organization}));

        /// <summary>
        ///     获取发送的目标id
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<Guid> GetTargetIds()
        {
            return SendType switch
            {
                SendModeType.User => UserIds ??= new List<Guid>(),
                SendModeType.Organization => OrganizationIds ??= new List<Guid>(),
                SendModeType.Role => RoleIds ??= new List<Guid>(),
                _ => null
            };
        }

        public List<Guid> GetUserIds() => UserIds ??= new List<Guid>();
        public IReadOnlyList<Guid> GetOrganizationIds() => OrganizationIds ??= new List<Guid>();
        public IReadOnlyList<Guid> GetRoleIds() => RoleIds ??= new List<Guid>();

        /// <summary>
        ///     设置消息内容
        /// </summary>
        /// <param name="content"></param>
        public virtual void SetContent<T>(T content) where T : MessageContent => Content = content;

        /// <summary>
        ///     序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual string Serialization() => JsonConvert.SerializeObject(Content);

        /// <summary>
        ///     根据类型获取对应的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Deserialization<T>() => JsonConvert.DeserializeObject<T>(Serialization());

        /// <summary>
        ///     获取当前对象的二进制数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetBinary() => this.GetBytes();
    }
}