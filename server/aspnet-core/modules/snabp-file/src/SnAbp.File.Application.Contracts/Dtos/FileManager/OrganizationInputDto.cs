/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： OrganizationInputDto
*******类 说 明： 组织结构信息查询dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/29 10:07:43
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class OrganizationInputDto:EntityDto<Guid>
    {
        public ResourceType Type { get; set; }
    }
}
