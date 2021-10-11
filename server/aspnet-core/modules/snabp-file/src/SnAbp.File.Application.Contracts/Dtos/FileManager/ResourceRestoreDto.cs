/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourceRestoreDto
*******类 说 明： 资源还原输入类，还原包括还原和还原到，可能是多个，可能是一个
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/17 13:41:32
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
    public class ResourceRestoreDto:EntityDto
    {
        public Guid[] FolderIds { get; set; }
        public Guid[] FileIds { get; set; }

        [Required] public Guid TargetId { get; set; }

        /// <summary>
        /// 文件移动或复制到的目标类型，具体参考<see cref="ResourceNodeType"/>,可以为空，为空表示还原本身
        /// </summary>
        public ResourceNodeType TargetType { get; set; }
    }
}
