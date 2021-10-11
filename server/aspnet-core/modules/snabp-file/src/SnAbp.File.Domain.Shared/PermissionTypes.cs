/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： PermissionType
*******类 说 明： 资源权限类型说明类，记录常用权限名称
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/8 15:15:21
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

namespace SnAbp.File
{
    /// <summary>
    ///     权限类型
    /// </summary>
    public static class PermissionTypes
    {
        /// <summary>
        ///     所有权限
        /// </summary>
        public static string All = "all";

        /// <summary>
        ///     编辑权限
        /// </summary>
        public static string Edit = "edit";

        /// <summary>
        ///     查看权限
        /// </summary>
        public static string View = "view";

        /// <summary>
        ///     删除权限
        /// </summary>
        public static string Delete = "delete";

        /// <summary>
        ///     引用权限
        /// </summary>
        public static string Quote = "quote";
    }
}