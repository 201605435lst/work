/**********************************************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： FileRltTag
*******类 说 明： 文件标签关联表，记录文件和标签的关联关系
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 14:03:12
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Entities
{
    public class FileRltTag : Entity<Guid>
    {
        public FileRltTag()
        {
        }

        public FileRltTag(Guid id)
        {
            Id = id;
        }

        public virtual Guid TagId { get; set; }

        public virtual Guid FileId { get; set; }
        /// <summary>
        ///     标签类
        /// </summary>
        public virtual Tag Tag { get; set; }

        /// <summary>
        ///     文件类
        /// </summary>
        public virtual File File { get; set; }
    }
}