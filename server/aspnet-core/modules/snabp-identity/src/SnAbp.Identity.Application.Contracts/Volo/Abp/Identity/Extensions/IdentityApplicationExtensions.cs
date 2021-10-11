/**********************************************************************
*******命名空间： Volo.Abp.Identity.Extensions
*******类 名 称： IdentityApplicationExtensions
*******类 说 明： 当前模块扩展方法
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/18 15:02:58
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnAbp.Identity
{
    public static class IdentityApplicationExtensions
    {
        /// <summary>
        /// 检查节点是否有子节点
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<OrganizationDto> CheckChildren(this List<OrganizationDto> list)
        {
            var newList = new List<OrganizationDto>();
            list?.ForEach(a =>
            {
                var model = CreateInstance(a);
                model.Children = a.Children.Any() ? new List<OrganizationDto>() : null;
                newList.Add(model);
            });
            return newList;
        }

        private static T CreateInstance<T>(T t) where T:class
        {
            var newT = Activator.CreateInstance<T>();
            foreach (var propertyInfo in newT.GetType().GetProperties())
            {
                var value = t.GetType().GetProperty(propertyInfo.Name)?.GetValue(t);
                propertyInfo.SetValue(newT, value);
            }
            return newT;
        }
    }
}
