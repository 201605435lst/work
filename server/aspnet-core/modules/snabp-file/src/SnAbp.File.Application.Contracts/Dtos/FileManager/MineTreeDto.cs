/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： MineTreeDto
*******类 说 明： 我的资源树结构，只返回当前用户上传的文件和文件夹
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/16 14:50:49
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
    public class MineTreeDto : EntityDto<Guid>
    {

        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public List<MineTreeDto> Children { get; set; }
        public string Field { get; set; }
        public int Type { get; set; }
    }
}
