using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace SnAbp.PermissionManagement
{
    public interface IPermissionManagementProvider : ISingletonDependency //TODO: Consider to remove this pre-assumption
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync(
            [NotNull] string name,
            [NotNull] string providerName,
            [NotNull] string providerKey
        );

        /// <summary>
        /// Easten 新增 合法性检查
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providerName"></param>
        /// <param name="providerGuid"></param>
        /// <returns></returns>
        Task<PermissionValueProviderGrantInfo> CheckAsync(
            [NotNull] string name,
            [NotNull] string providerName,
            [NotNull] Guid providerGuid
        );

        Task SetAsync(
            [NotNull] string name,
            [NotNull] string providerKey,
            bool isGranted
        );

        Task SetAsync(
            [NotNull] string name,
            [NotNull] Guid providerGuid,
            bool isGranted
        );
    }
}