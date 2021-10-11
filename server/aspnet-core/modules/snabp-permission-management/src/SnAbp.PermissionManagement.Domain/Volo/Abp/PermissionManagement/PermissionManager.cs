using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace SnAbp.PermissionManagement
{
    public class PermissionManager : IPermissionManager, ISingletonDependency
    {
        private readonly IRepository<PermissionGrant, Guid> _permissionRepository;
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected IGuidGenerator GuidGenerator { get; }

        protected ICurrentTenant CurrentTenant { get; }

        protected IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;

        protected PermissionManagementOptions Options { get; }

        private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders;

        public PermissionManager(
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IRepository<PermissionGrant, Guid> permissionRepository,
            IServiceProvider serviceProvider,
            IGuidGenerator guidGenerator,
            IOptions<PermissionManagementOptions> options,
            ICurrentTenant currentTenant)
        {
            GuidGenerator = guidGenerator;
            CurrentTenant = currentTenant;
            _permissionRepository = permissionRepository;
            PermissionGrantRepository = permissionGrantRepository;
            PermissionDefinitionManager = permissionDefinitionManager;
            Options = options.Value;

            _lazyProviders = new Lazy<List<IPermissionManagementProvider>>(
                () => Options
                    .ManagementProviders
                    .Select(c => serviceProvider.GetRequiredService(c) as IPermissionManagementProvider)
                    .ToList(),
                true
            );
        }

        public virtual async Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, Guid providerGuid)
        {
            return await GetInternalAsync(PermissionDefinitionManager.Get(permissionName), providerName, providerGuid);
        }

        public virtual async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string providerName, Guid providerGuid)
        {
            var results = new List<PermissionWithGrantedProviders>();

            foreach (var permissionDefinition in PermissionDefinitionManager.GetPermissions())
            {
                results.Add(await GetInternalAsync(permissionDefinition, providerName, providerGuid));
            }

            return results;
        }

        public virtual async Task<bool> IsGranted(string providerName, Guid providerGuid)
        {
            var isGranted = false;

            foreach (var permissionDefinition in PermissionDefinitionManager.GetPermissions())
            {
                isGranted = _permissionRepository.FirstOrDefault(x =>
                 x.Name == permissionDefinition.Name &&
                 x.ProviderGuid == providerGuid &&
                 x.ProviderName == providerName) != null;
                if (isGranted) break;
            }

            return isGranted;
        }

        public virtual async Task SetAsync(string permissionName, string providerName, Guid providerGuid, bool isGranted)
        {
            var permission = PermissionDefinitionManager.Get(permissionName);

            if (!permission.IsEnabled)
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' is disabled!");
            }

            if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' has not compatible with the provider named '{providerName}'");
            }

            if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' has multitenancy side '{permission.MultiTenancySide}' which is not compatible with the current multitenancy side '{CurrentTenant.GetMultiTenancySide()}'");
            }

            var currentGrantInfo = await GetInternalAsync(permission, providerName, providerGuid);
            if (currentGrantInfo.IsGranted == isGranted)
            {
                return;
            }

            var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
            if (provider == null)
            {
                //TODO: BusinessException
                throw new AbpException("Unknown permission management provider: " + providerName);
            }

            await provider.SetAsync(permissionName, providerGuid, isGranted);
        }

        public virtual async Task<PermissionGrant> UpdateProviderKeyAsync(PermissionGrant permissionGrant, string providerKey)
        {
            permissionGrant.ProviderKey = providerKey;
            return await PermissionGrantRepository.UpdateAsync(permissionGrant);
        }

        /// <summary>
        /// 更新权限提供者id Easten新增
        /// </summary>
        /// <param name="permissionGrant"></param>
        /// <param name="providerGuid"></param>
        /// <returns></returns>
        public virtual async Task<PermissionGrant> UpdateProviderGuidAsync(PermissionGrant permissionGrant, Guid providerGuid)
        {
            permissionGrant.ProviderGuid = providerGuid;
            return await PermissionGrantRepository.UpdateAsync(permissionGrant);
        }


        protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition permission, string providerName, Guid providerGuid)
        {
            var result = new PermissionWithGrantedProviders(permission.Name, false);

            if (!permission.IsEnabled)
            {
                return result;
            }

            if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
            {
                return result;
            }

            if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
            {
                return result;
            }

            foreach (var provider in ManagementProviders)
            {
                var providerResult = await provider.CheckAsync(permission.Name, providerName, providerGuid);
                if (providerResult.IsGranted)
                {
                    result.IsGranted = true;
                    result.Providers.Add(new PermissionValueProviderInfo(provider.Name, providerResult.ProviderGuid.GetValueOrDefault()));
                }
            }

            return result;
        }
    }
}