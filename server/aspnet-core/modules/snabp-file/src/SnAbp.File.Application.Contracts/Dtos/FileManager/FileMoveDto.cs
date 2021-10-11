/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileMoveDto
*******类 说 明： 文件操作dto ,用来作为文件移动、文件、复制的输入对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 16:20:24
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileOperationDto : EntityDto<Guid>
    {
        /// <summary>
        /// 移动或复制的目标id,可以时直接的组织目录，可以是某一个文件夹的id
        /// </summary>
        [Required] public Guid TargetId { get; set; }

        /// <summary>
        /// 文件移动或复制到的目标类型，具体参考<see cref="ResourceNodeType"/>
        /// </summary>
        [Required] public ResourceNodeType TargetType { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        [Required] public ResourceType Type { get; set; }
    }
}
