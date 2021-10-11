/**********************************************************************
*******命名空间： SnAbp.Utils
*******类 名 称： EntityBuilderHandler
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/21 11:49:21
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SnAbp.Utils
{
    /// <summary>
    /// $$
    /// </summary>
    public class EntityBuilderHandler
    {
        public static IEnumerable<Type> GetCurrentDomainDbEntities(Type dbProperties)
        {
            var baseType = Assembly.GetAssembly(dbProperties);
            if (!baseType.GetName().Name.Contains("Domain")) return Enumerable.Empty<Type>();
            var entityTypes = baseType
                .GetTypes()
                .Where(a => a.BaseType.IsGenericType)
                .ToList();
            return entityTypes ?? Enumerable.Empty<Type>();
        }
    }
}
