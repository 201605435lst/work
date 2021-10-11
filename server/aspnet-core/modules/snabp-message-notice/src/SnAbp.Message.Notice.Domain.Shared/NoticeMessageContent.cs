/**********************************************************************
*******命名空间： SnAbp.Message.Notice
*******类 名 称： NoticeMessageContent
*******类 说 明： 通知消息内容实体定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 16:21:48
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SnAbp.Message.MessageDefine;

namespace SnAbp.Message.Notice
{
    [Serializable]
    public class NoticeMessageContent:MessageContent
    {
        /// <summary>
        /// 消息时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息的类型，各模块自己定义，并实现，例如：工作汇报就使用：关键字report, 栏目文章使用article等
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 通知发起人
        /// </summary>
        public Guid SponsorId { get; set; }

        /// <summary>
        /// 消息的内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void CreateContent<T>(T t) => this.Content = JsonConvert.SerializeObject(t);
    }
}
