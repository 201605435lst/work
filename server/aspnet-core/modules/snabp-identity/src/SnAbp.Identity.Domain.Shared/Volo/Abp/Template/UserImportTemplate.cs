/**********************************************************************
*******命名空间： Volo.Abp
*******类 名 称： UserImportTemplate
*******类 说 明： 用户导入模板
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/22 11:36:51
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Identity
{
    public class UserImportTemplate
    {
        public int Index { get; set; }

        public string OrgCode { get; set; }

        public string OrgName { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
    }
}
