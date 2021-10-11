/**********************************************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： FolderRltShare
*******类 说 明： 文件夹共享关联表，包含权限信息
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 14:18:48
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using SnAbp.Identity;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Entities
{
    public class FolderRltShare : Entity<Guid>
    {
        public FolderRltShare()
        {
        }
        public void SetId(Guid id)
        {
            this.Id = id;
        }
        public FolderRltShare(Guid id)
        {
            Id = id;
        }

        /// <summary>
        ///     文件夹id
        /// </summary>
        public virtual Guid FolderId { get; set; }

        /// <summary>
        ///     文件夹对象
        /// </summary>
        public virtual Folder Folder { get; set; }

        /// <summary>
        ///     成员id，指定具体的组织，角色或者用户
        /// </summary>
        public virtual Guid? MemberId { get; set; }

        /// <summary>
        ///     成员类型，对应组织，角色，用户
        /// </summary>
        public virtual MemberType Type { get; set; }

        /// <summary>
        ///     编辑权限
        /// </summary>
        public virtual bool Edit { get; set; }

        /// <summary>
        ///     查看权限
        /// </summary>
        public virtual bool View { get; set; }

        /// <summary>
        ///     删除权限
        /// </summary>
        public virtual bool Delete { get; set; }

        /// <summary>
        ///     使用权限
        /// </summary>
        public virtual bool Use { get; set; }
    }
}