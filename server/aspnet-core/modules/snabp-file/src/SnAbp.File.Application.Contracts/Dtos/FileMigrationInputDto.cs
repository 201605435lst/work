/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileMigrationInputDto
*******类 说 明： 文件迁移输入
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 13:38:24
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File.Dtos
{
    public class FileMigrationInputDto
    {
        [NotNull] public Guid SrouceId { get; set; }
        [NotNull] public Guid TargetId { get; set; }
    }
}
