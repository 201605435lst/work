using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace SnAbp.PermissionManagement
{
    public class PermissionValueProviderInfo
    {
        public string Name { get; }

      //  public string Key { get; }

        public Guid Guid { get; }
        //public PermissionValueProviderInfo([NotNull]string name, [NotNull]string key)
        //{
        //    Check.NotNull(name, nameof(name));
        //    Check.NotNull(key, nameof(key));

        //    Name = name;
        //    Key = key;
        //}
        public PermissionValueProviderInfo([NotNull] string name, [NotNull] Guid guid)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(guid, nameof(guid));

            Name = name;
            Guid = guid;
        }
    }
}