/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FolderInputDto
*******类 说 明： 文件夹输入对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 17:42:50
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileFolderInputDto : EntityDto<Guid>
    {
        /// <summary>
        ///     组织Id
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        ///     父节点的Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        ///     文件夹名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     指定文件夹key
        /// </summary>
        public string StaticKey { get; set; }
    }
}