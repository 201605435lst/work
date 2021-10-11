using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SnAbp.SettingManagement;
using SnAbp.Users;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;

namespace SnAbp.Identity
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(SnAbpSettingManagementDomainModule),
        typeof(SnAbpIdentityDomainSharedModule),
        typeof(SnAbpUsersDomainModule),
        typeof(AbpAutoMapperModule)
        )]
    public class SnAbpIdentityDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SnAbpIdentityDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<IdentityDomainMappingProfile>(validate: true);
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<IdentityUser, UserEto>(typeof(SnAbpIdentityDomainModule));
                options.EtoMappings.Add<IdentityClaimType, IdentityClaimTypeEto>(typeof(SnAbpIdentityDomainModule));
                options.EtoMappings.Add<IdentityRole, IdentityRoleEto>(typeof(SnAbpIdentityDomainModule));
                options.EtoMappings.Add<Organization, OrganizationEto>(typeof(SnAbpIdentityDomainModule));
            });
            
            var identityBuilder = context.Services.AddAbpIdentity(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            context.Services.AddObjectAccessor(identityBuilder);
            context.Services.ExecutePreConfiguredActions(identityBuilder);

            AddAbpIdentityOptionsFactory(context.Services);
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.User,
                typeof(IdentityUser)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.Role,
                typeof(IdentityRole)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.ClaimType,
                typeof(IdentityClaimType)
            );
            
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.Organization,
                typeof(Organization)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.DataDictionary,
                typeof(DataDictionary)
            );
        }

        private static void AddAbpIdentityOptionsFactory(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IOptionsFactory<IdentityOptions>, SnAbpIdentityOptionsFactory>());
            services.Replace(ServiceDescriptor.Scoped<IOptions<IdentityOptions>, OptionsManager<IdentityOptions>>());
        }
    }
}