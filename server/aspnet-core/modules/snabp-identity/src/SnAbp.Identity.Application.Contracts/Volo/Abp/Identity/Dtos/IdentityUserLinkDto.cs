/**********************************************************************
*******命名空间： Volo.Abp.Identity.Dtos
*******类 名 称： IdentityUserLinkDto
*******类 说 明： 用户关联dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/21 18:39:47
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Identity
{
    public class IdentityUserLinkDto
    {
        public Guid? PositionId { get; set; }
        public Guid? UserId { get; set; }
    }

    public class IdentityUserSetDto
    {
        public Guid OrganizationId { get; set; }
        public List<IdentityUserLinkDto> UserAndPositions { get; set; }
    }
}
