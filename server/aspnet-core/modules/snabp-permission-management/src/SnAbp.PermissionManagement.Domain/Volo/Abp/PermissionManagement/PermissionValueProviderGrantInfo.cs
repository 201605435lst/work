using System;
using JetBrains.Annotations;

namespace SnAbp.PermissionManagement
{
    public class PermissionValueProviderGrantInfo //TODO: Rename to PermissionGrantInfo
    {
        public static PermissionValueProviderGrantInfo NonGranted { get; } = new PermissionValueProviderGrantInfo(false,"");

        public virtual bool IsGranted { get; }

        public virtual string ProviderKey { get; }

        // Easten新增
        public virtual Guid? ProviderGuid { get; }

        public PermissionValueProviderGrantInfo(bool isGranted, [CanBeNull] string providerKey = null)
        {
            IsGranted = isGranted;
            ProviderKey = providerKey;
        }

        // Easten 新增
        public PermissionValueProviderGrantInfo(bool isGranted, [CanBeNull] Guid?providerGuid = null)
        {
            IsGranted = isGranted;
            ProviderGuid = providerGuid;
        }
    }
}