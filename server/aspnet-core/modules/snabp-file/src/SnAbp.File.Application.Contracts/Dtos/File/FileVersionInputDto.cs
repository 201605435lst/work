/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileVersionInputDto
*******类 说 明： 文件版本保存dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/30 16:26:41
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
    public class FileVersionInputDto:EntityDto<Guid>
    {
        /// <summary>
        ///  选中的需要关联的文件版本
        /// </summary>
        public Guid SelectId { get; set; }
    }
}
