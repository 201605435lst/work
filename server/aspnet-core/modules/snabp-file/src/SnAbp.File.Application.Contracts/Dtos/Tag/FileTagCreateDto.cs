/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： TagCreateDto
*******类 说 明： 标签创建类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 8:41:41
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileTagCreateDto : EntityDto<Guid>
    {
        [Required] public Guid OrganizationId { get; set; }

        [Required] public string Name { get; set; }
    }
}