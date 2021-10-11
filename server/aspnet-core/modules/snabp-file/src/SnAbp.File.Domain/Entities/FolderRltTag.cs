/**********************************************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： FolderRltTag
*******类 说 明： 文件夹标签关联表，用来记录文件夹的标签信息
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 14:05:16
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Entities
{
    public class FolderRltTag : Entity<Guid>
    {
        public FolderRltTag()
        {
        }

        public FolderRltTag(Guid id)
        {
            Id = id;
        }

        public  Guid TagId { get; set; }
        public  Guid FolderId { get; set; }

        /// <summary>
        ///     标签类
        /// </summary>
        public virtual Tag Tag { get; set; }

        /// <summary>
        ///     文件夹对象
        /// </summary>
        public virtual Folder Folder { get; set; }
    }
}