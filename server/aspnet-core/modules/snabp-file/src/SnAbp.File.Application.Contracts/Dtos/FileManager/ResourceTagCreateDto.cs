/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourceTagCreateDto
*******类 说 明： 资源标签输入dto,用来支持多资源，标签的数据输入
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 9:11:06
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
    public class ResourceTagCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 资源集合
        /// </summary>
        [Required] public List<ResourceTagInputDto> Resources { get; set; }

        /// <summary>
        /// 标签Id集合
        /// </summary>
        [Required] public List<Guid> TagIds { get; set; }
    }
}
