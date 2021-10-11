/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FolderDto
*******类 说 明： 获取文件夹Dto,一个文件夹对象要返回：自己的标签、权限、包含的子文件及文件夹，文件夹大小
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 17:50:09
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileFolderDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        /// <summary>
        ///     是否被分享
        /// </summary>
        public bool IsShare { get; set; }

        /// <summary>
        ///     是否公开
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        ///     文件夹路径
        ///     路径格式：文件夹1/文件夹2/文件夹3/..
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     指定文件夹key
        /// </summary>
        public string StaticKey { get; set; }

        /// <summary>
        ///     文件夹大小，需要遍历计算出其子文件的所有大小
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        ///     类型 ，指定文件或者文件夹
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 组织结构的id
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }
    }
}