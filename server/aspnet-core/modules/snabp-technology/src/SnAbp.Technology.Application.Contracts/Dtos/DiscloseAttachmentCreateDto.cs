/**********************************************************************
*******命名空间： SnAbp.Technology.Dtos
*******类 名 称： DiscloseAttachmentCreateDto
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/1 10:55:07
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Technology.Dtos
{
    /// <summary>
    /// $$
    /// </summary>
    public class DiscloseAttachmentCreateDto:EntityDto<Guid>
    {
        public DiscloseAttachmentCreateDto() { }

        public List<DiscloseCreateDto> Items { get; set; }
        public Guid ParentId { get; set; }
    }
}
