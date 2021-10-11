/**********************************************************************
*******命名空间： Volo.Abp.Identity.Setting
*******类 名 称： SystemSettings
*******类 说 明： 常量定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/14 15:59:39
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Identity
{
    public class SystemSettings
    {
        // public const string GroupName = "System";

        /* Add constants for setting names. Example:
         * public const string MySettingName = GroupName + ".MySettingName";
         */

        public const string DbTablePrefix = "Sn_App_";
        public const string DbSchema = null;



        #region 通用导入常量
        public const string DataImportRowFlag = "[SeenSun]";
        public const string Name = "[Name]";
        #endregion

        #region 组织机构导入常量
        public const string CSRGCode = "[CSRGCode]";
        public const string Nature = "[Nature]";

        #endregion
    }
}
