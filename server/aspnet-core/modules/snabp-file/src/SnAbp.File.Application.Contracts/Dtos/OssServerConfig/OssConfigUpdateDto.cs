/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： OssConfigUpdateDto
*******类 说 明： oss 配置更新类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 8:35:14
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.ComponentModel.DataAnnotations;
using SnAbp.File.OssSdk;

namespace SnAbp.File.Dtos
{
    public class OssConfigUpdateDto
    {
        [Required] public Guid Id { get; set; }

        public string EndPoint { get; set; }
        public string AccessSecret { get; set; }
        public string AccessKey { get; set; }
        public string Type { get; set; }
    }
}