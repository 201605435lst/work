/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourceTagInputDto
*******类 说 明： 资源标签保存数据输入对象，记录不同资源的数据类型
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 9:14:37
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Dtos
{
    [NotMapped]
    public class ResourceTagInputDto:EntityDto<Guid>
    {
        /// <summary>
        /// 资源类型
        /// <code>文件夹：Type=null</code>
        /// <code>文件：Type=具体的type</code>
        /// </summary>
        public ResourceType Type { get; set; }
    }
}
