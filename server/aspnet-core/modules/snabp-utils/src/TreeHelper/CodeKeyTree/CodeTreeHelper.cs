using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnAbp.Utils.TreeHelper
{
    public class CodeTreeHelper<T> where T : ICodeTree<T>
    {
        /// <summary>
        /// 根据分隔符获取树状数组
        /// </summary>
        /// <param name="list"></param> 原始数组
        /// <param name="separator"></param> 分隔符
        /// <returns></returns>
        public static List<T> GetTree(List<T> list, char[] separator)
        {
            foreach (var item in list)
            {
                if (item.Children != null)
                {
                    item.Children = new List<T>();
                }
            }

            foreach (var item in list)
            {
                // 获取父级 code
                var parentCode = "";
                var array = new List<string>(item.Code.Split(separator));
                array.RemoveAt(array.Count - 1);

                var index = 0;
                foreach (var code in array)
                {
                    parentCode += code + (index < array.Count - 1 ? new string(separator) : "");
                    index++;
                }

                // 找 item 的 parent，把 item 加入到 parent 的 children
                foreach (var target in list)
                {
                    if (target.Code == parentCode)
                    {
                        target.Children = target.Children ?? new List<T>();
                        target.Children.Add(item);
                        item.Parent = target;
                        break;
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
            return list.Where(x => x.Parent == null).ToList();
        }


        public static List<T> GetTree(List<T> list, string separator)
        {
            return GetTree(list, separator.ToCharArray());
        }

        /// <summary>
        /// 根据截断长度获取树状数组
        /// </summary>
        /// <param name="list"></param>原始数组
        /// <param name="length"></param>Code 分割长度
        /// <returns></returns>
        public static List<T> GetTree(List<T> list, int length)
        {
            foreach (var item in list)
            {
                foreach (var target in list)
                {
                    if (item.Code.Length - target.Code.Length == length && item.Code.StartsWith(target.Code))
                    {
                        target.Children = target.Children ?? new List<T>();
                        target.Children.Add(item);
                        item.Parent = target;
                        break;
                    }
                }
            }
            return list.Where(x => x.Parent == null).ToList();
        }

        /// <summary>
        /// 根据截断长度获取树状数组
        /// </summary>
        /// <param name="list"></param>原始数组
        /// <param name="length"></param>Code 分割长度
        /// <returns></returns>
        public static List<T> GetTree(List<T> list, int maxLength, int minLength)
        {
            foreach (var item in list)
            {
                foreach (var target in list)
                {
                    var length = item.Code.Length - target.Code.Length;
                    if (length >= minLength && length <= maxLength && item.Code.StartsWith(target.Code))
                    {
                        target.Children = target.Children ?? new List<T>();
                        target.Children.Add(item);
                        item.Parent = target;
                        break;
                    }
                }
            }
            return list.Where(x => x.Parent == null).ToList();
        }
    }
}
