/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourcePermissionInputDto
*******类 说 明： 资源权限输入对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/17 15:23:36
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
    public class ResourcePermissionInputDto:EntityDto
    {
       [Required] public Guid Id { get; set; }
       [Required] public ResourceType Type { get; set; }
    }
}
