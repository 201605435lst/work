/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FolderUpadteDto
*******类 说 明： 文件更新dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 17:48:19
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileFolderUpdateDto : EntityDto<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}