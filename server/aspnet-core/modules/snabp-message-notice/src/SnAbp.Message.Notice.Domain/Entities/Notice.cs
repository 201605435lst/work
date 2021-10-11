/**********************************************************************
*******命名空间： SnAbp.Message.Notice.Entities
*******类 名 称： NoticeMessage
*******类 说 明： 通知类消息实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/25 10:13:10
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using SnAbp.Identity;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Message.Notice.Entities
{
    public class Notice:AuditedEntity<Guid>
    {
        public Notice(Guid id) => this.Id = id;
        /// <summary>
        /// 消息的接收人
        /// </summary>
        public virtual Guid UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        /// <summary>
        /// 消息的类型，若是系统消息，类型为:system,其他的类型都由各模块自定义，并在前端做响应的实现
        /// </summary>
        public virtual string Type { get; set; }
        
        /// <summary>
        /// 流程是否处理或已读
        /// </summary>
        public virtual bool Process { get; set; }

        /// <summary>
        /// 消息的内容，由各模块自定义dto,并序列化后的结果，前端自定义处理；
        /// </summary>
        public virtual string Content { get; set; }
        
    }
}
