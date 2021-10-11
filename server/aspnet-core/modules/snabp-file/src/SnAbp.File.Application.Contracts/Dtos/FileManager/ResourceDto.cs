/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourceDto
*******类 说 明： 数据资源兑对象，根据资源id查询的资源列表，支持分表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 9:43:07
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.File.Entities;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class ResourceDto:EntityDto<Guid>
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 资源类型,如果是文件，则显示文件的后缀名
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 修改日期，记录最新的修改时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 资源类型，标识该资源是文件还是文件夹
        /// 具体类型值查看<see cref="SnAbp.File.ResourceType"></see>
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// 资源的标签组
        /// </summary>
        public List<Entities.Tag> Tags { get; set; }

        /// <summary>
        /// 文件版本，文件夹不存在此属性
        /// </summary>
        public List<FileVersion> Versions { get; set; }

        /// <summary>
        /// 文件共享权限
        /// </summary>
        public List<FileRltShare> FileShares { get; set; }

        /// <summary>
        /// 文件夹的共享权限
        /// </summary>
        public List<FolderRltShare> FolderShares { get; set; }

        /// <summary>
        /// 文件权限信息
        /// </summary>
        public  List<FileRltPermissions> FilePermissions { get; set; }

        /// <summary>
        /// 文件夹的权限信息
        /// </summary>
        public List<FolderRltPermissions> FolderPermissions { get; set; }
        /// <summary>
        /// 是否共享
        /// </summary>
        public bool IsShare { get; set; }

        // TODO 添加资源的权限信息

        public string Url { get; set; }
    }
}
