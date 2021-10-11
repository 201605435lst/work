/**********************************************************************
*******命名空间： SnAbp.Safe.Entities
*******类 名 称： SafeSpeechVideo
*******类 说 明： 班前讲话视频
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/7 14:53:44
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Safe.Entities
{
    /// <summary>
    /// 班前讲话视频
    /// </summary>
    public class SafeSpeechVideo : AuditedEntity<Guid>
    {
        public SafeSpeechVideo(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;

        /// <summary>
        /// 项目id
        /// </summary>
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        /// <summary>
        /// 施工部位
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// 施工内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 施工日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 讲话视频
        /// </summary>
        public virtual File.Entities.File Video { get; set; }
        public virtual Guid? VideoId { get; set; }

    }
}
