/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： ResourceDeleteDto
*******类 说 明： 资源删除Dto 定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 18:08:01
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
    public class ResourceDeleteDto:EntityDto<Guid>
    {
        public List<Guid> Folders { get; set; }
        public List<Guid> Files { get; set; }

        /// <summary>
        /// 是否真实删除，用来标记回收站中的文件删除
        /// </summary>
        public bool IsTure { get; set; }
    }
}
