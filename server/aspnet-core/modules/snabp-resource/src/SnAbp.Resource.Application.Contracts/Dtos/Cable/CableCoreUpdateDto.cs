/**********************************************************************
*******命名空间： SnAbp.Resource.Dtos.Cable
*******类 名 称： CableCoreUpdateDto
*******类 说 明： 线芯更新实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/10/29 13:36:32
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Resource.Enums;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class CableCoreUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 线芯类型
        /// </summary>
        public CableCoreType Type { get; set; }
    }
}
