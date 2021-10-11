/**********************************************************************
*******命名空间： SnAbp.Resource.Dtos.Terminal
*******类 名 称： TerminalLinkUpdateDto
*******类 说 明： 端子关联关系
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/10/29 9:38:25
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Resource.Dtos
{
    public class TerminalLinkUpdateDto : EntityDto
    {
        /// <summary>
        /// 关系id
        /// </summary>
        public Guid TerminalLinkId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessFunction { get; set; }
    }
}
