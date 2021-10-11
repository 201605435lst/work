using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SnAbp.Utils.TreeHelper
{
    public class TreeHelper<KeyT, T> where T : ITree<KeyT, T>
    {
        public static List<T> GetTree(List<T> list)
        {
            foreach (var item in list)
            {
                item.Children = new List<T>();
            }
            foreach (var item in list)
            {
                // 找 item 的 parent，把 item 加入到 parent 的 children
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
            return list.Where(x => string.IsNullOrEmpty(x.ParentId.ToString())).ToList();
        }
    }

    public static class TreeHelper
    {
        static BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public static List<T> GetTree<T, Tkey>(this List<T> list, string key, string parentKey, string childrenKey)
        {
            foreach (var item in list)
            {
                SetValue(item, childrenKey, new List<T>());
            }

            var rootList = new List<T>();
            foreach (var item in list)
            {
                var itemParentKey = GetValue<Tkey>(item, parentKey);
                var hasParent = false;
                foreach (var target in list)
                {
                    var targetKey = GetValue<Tkey>(target, key);
                    if (targetKey.Equals(itemParentKey))
                    {
                        var children = GetValue<List<T>>(target, childrenKey);
                        if(children == null)
                        {
                            children = new List<T>();
                        }
                        children.Add(item);
                        SetValue(target, childrenKey, children);
                        hasParent = true;
                        break;
                    }
                }
                if (!hasParent)
                {
                    rootList.Add(item);
                }
            }
            return rootList;
        }

        public static T GetValue<T>(object obj, string name)
        {
            object value = null;
            if (obj != null)
            {

                var prop = obj.GetType().GetProperties(Flags).Where(x => x.Name == name).FirstOrDefault();
                value = prop?.GetValue(obj);
            }
            if(value == null)
            {
                return default(T);
            }
            return (T)value;
        }

        public static void SetValue(object obj, string name, object value)
        {
            if (obj != null && value != null)
            {
                var prop = obj.GetType().GetProperties(Flags).Where(x => x.Name == name).FirstOrDefault();
                prop?.SetValue(obj, value);
            }
        }
    }
}
