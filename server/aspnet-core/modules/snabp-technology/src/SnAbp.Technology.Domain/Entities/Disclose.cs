/**********************************************************************
*******命名空间： SnAbp.Technology.Entities
*******类 名 称： Disclose
*******类 说 明： 技术交底信息表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/26 16:02:42
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
namespace SnAbp.Technology.Entities
{
    /// <summary>
    /// 技术交底信息表
    /// </summary>
    public class Disclose : AuditedEntity<Guid>
    {
        public Disclose(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;

        /// <summary>
        /// 视频名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 视频地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
        public Disclose Parent { get; set; }
        public DiscloseType Type { get; set; }

        public Guid? ProjectTagId { get; set; }
    }
}
