/**********************************************************************
*******命名空间： Volo.Abp.Template
*******类 名 称： OrganizationImportTemplate
*******类 说 明： 组织机构导入模板
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/23 17:21:12
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Identity
{
    public class OrganizationImportTemplate
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string CSRGCode { get; set; }
        public string Nature { get; set; }
        public string IsKeshi { get; set; }
    }
}
