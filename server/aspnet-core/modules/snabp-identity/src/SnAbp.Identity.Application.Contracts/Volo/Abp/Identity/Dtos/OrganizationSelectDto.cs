/**********************************************************************
*******命名空间： Volo.Abp.Identity.Dtos
*******类 名 称： OrganizationSelectDto
*******类 说 明： 组织机构选择dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/14 15:35:24
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Identity
{
    public class OrganizationSelectDto
    {
        public string Name { get; set; }
        public string CSRGCode { get; set; }
        public string Code { get; set; }
        public string Nature { get; set; }
        public List<OrganizationSelectDto> Children { get; set; } = new List<OrganizationSelectDto>();
        public OrganizationSelectDto Parent { get; set; }
    }
}
