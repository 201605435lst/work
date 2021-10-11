using System;
using System.Collections.Generic;
using System.Linq;

namespace SnAbp.Utils.TreeHelper
{
    public class GuidKeyTreeHelper<T> where T : IGuidKeyTree<T>
    {

        /// <summary>
        /// 构件树
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> GetTree(List<T> list)
        {
            foreach (var item in list)
            {
                // 原始数组子元素清零
                if (item.Children != null)
                {
                    item.Children = new List<T>();
                }
            }
            foreach (var item in list)
            {
                // 找 item 的 parent，把 item 加入到 parent 的 children
                if (item.ParentId.HasValue)
                {
                    foreach (var target in list)
                    {
                        if (item.ParentId.ToString() == target.Id.ToString())
                        {
                            target.Children = target.Children ?? new List<T>();
                            target.Children.Add(item);
                            item.Parent = target;
                            break;
                        }
                    }
                }
            }
            foreach (var item in list)
            {
                if (item.Children != null)
                {
                    item.Children = item.Children.Distinct().ToList();
                }
            }
            var ids = list.Select(x => x.Id.ToString());
            return list.Where(x => !ids.Contains(x.ParentId.ToString())).ToList();
        }


        /// <summary>
        /// 获取父节点集合
        /// </summary>
        /// <param name="list">原始数组</param>
        /// <param name="target">目标元素</param>
        /// <param name="withSibling">是否包含同级节点</param>
        /// <returns></returns>
        public static List<T> GetParents(List<T> list, T target, bool withSibling = true)
        {
            var result = new List<T>();
            if (withSibling)
            {
                result.Add(target);
                if (target?.ParentId != null)
                {
                    result.AddRange(list.Where(x => x.ParentId == target?.ParentId));
                }
            }

            _GetParents(list, result, target, withSibling);
            return result.Distinct().ToList();
        }

        public static void _GetParents(List<T> list, List<T> result, T target, bool withSibling = true)
        {
            var parent = list.Find(x => x.Id == target?.ParentId);
            if (parent != null)
            {
                result.Add(parent);

                if (withSibling)
                {
                    result.AddRange(list.Where(x => x.ParentId == target?.ParentId).ToList());
                }
                _GetParents(list, result, parent, withSibling);
            }
            //else
            //{
            //    if (withSibling)
            //    {
            //        if(target.ParentId != null)
            //        {
            //            result.AddRange(list.Where(x => x.ParentId == target.ParentId));
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 获取子节点集合
        /// </summary>
        /// <param name="list">原始数组</param>
        /// <param name="target">目标元素</param>
        /// <returns></returns>
        public static List<T> GetChildren(List<T> list, Guid targetId)
        {
            var result = new List<T>();
            _GetChildren(list, result, targetId);
            return result.Distinct().ToList();
        }

        public static void _GetChildren(List<T> list, List<T> result, Guid targetId)
        {
            var children = list.Where(x => x.ParentId == targetId).ToList();
            if (children.Count > 0)
            {
                result.AddRange(children);
                foreach (var item in children)
                {
                    _GetChildren(list, result, item.Id);
                }
            }
        }
    }
}
