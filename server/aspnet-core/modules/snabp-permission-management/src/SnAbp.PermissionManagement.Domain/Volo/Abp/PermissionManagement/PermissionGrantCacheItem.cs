using System;

namespace SnAbp.PermissionManagement
{
    [Serializable]
    public class PermissionGrantCacheItem
    {
        public bool IsGranted { get; set; }

        public PermissionGrantCacheItem()
        {

        }

        public PermissionGrantCacheItem(bool isGranted)
        {
            IsGranted = isGranted;
        }
        /// <summary>
        /// 计算快捷键  Easten改造
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providerName"></param>
        /// <param name="providerGuid"></param>
        /// <returns></returns>
        public static string CalculateCacheKey(string name, string providerName, Guid providerGuid)
        {
           return $"pn:{providerName},pid:{providerGuid},n:{name}";
        }
        public static string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return "pn:" + providerName + ",pk:" + providerKey + ",n:" + name;
        }
    }
}