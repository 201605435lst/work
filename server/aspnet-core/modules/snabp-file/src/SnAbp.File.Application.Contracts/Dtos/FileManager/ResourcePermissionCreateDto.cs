/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourcePermissionCreateDto
*******类 说 明： 资源权限创建输入对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/17 15:25:35
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class ResourcePermissionCreateDto:EntityDto<Guid>
    {
        /// <summary>
        /// 组织结构id集合
        /// </summary>
        [CanBeNull] public Guid[] Organizations { get; set; }
        /// <summary>
        /// 角色集合
        /// </summary>
        [CanBeNull] public Guid[] Rolers { get; set; }
        /// <summary>
        /// 用户集合
        /// </summary>
        [CanBeNull] public Guid[] Users { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        [Required] public ResourceType Type { get; set; }

        /// <summary>
        /// 编辑权限
        /// </summary>
        public bool Edit { get; set; }

        /// <summary>
        ///  查看权限
        /// </summary>
        public bool View { get; set; }

        /// <summary>
        ///  删除权限
        /// </summary>
        public bool Delete { get; set; }

        /// <summary>
        ///  使用权限
        /// </summary>
        public bool Use { get; set; }
    }
}
