/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： EntityMapExtensions
*******类 说 明： 实体映射及常用方法扩展类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 17:07:24
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace SnAbp.File
{
    public static class EntityMapExtensions
    {
        /// <summary>
        /// 将<see cref="IQueryable" /> 对象转换成指定的泛型集合
        /// </summary>
        /// <typeparam name="T1">目标实体</typeparam>
        /// <typeparam name="T2">IQueryable对象</typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<List<T1>> ToMapperList<T1, T2>(this IQueryable<T2> query) where T1 : new()
        {
            // 构建映射
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T1, T2>();
                cfg.CreateMap<T2, T1>();
            });
            var mapper = configuration.CreateMapper();
            var list = new List<T1>();
            if (!query.Any())
            {
                return await Task.FromResult(list);
            }

            foreach (var item in query)
            {
                var t1 = mapper.Map<T1>(item);
                list.Add(t1);
            }

            return await Task.FromResult(list);
        }

        /// <summary>
        /// Guid集合对象转换成<example>Guid?[]</example>数组格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Guid?[] ToGuidArray<T>(this IEnumerable<T> tSource) where  T: IFormattable
        {
            var list = tSource.ToList();
            if (list.Count()>0)
            {
                var guids = new Guid?[list.Count()];
                for (var i = 0; i < list.Count(); i++)
                {
                    Guid.TryParse(list[i].ToString(), out var guid);
                    guids[i] = guid;
                }
                return guids;
            }
            else
            {
                return null;
            }
        }
    }
}