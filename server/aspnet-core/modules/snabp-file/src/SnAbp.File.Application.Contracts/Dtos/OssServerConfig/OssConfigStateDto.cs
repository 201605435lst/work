/**********************************************************************
*******命名空间： SnAbp.File.Dtos.OssServerConfig
*******类 名 称： OssConfigStateDto
*******类 说 明： 文件服务管理状态dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/21 9:01:20
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File.Dtos
{
    public class OssConfigStateDto
    {
        public  Guid Id { get; set; }
        public  string State { get; set; }
    }
}
