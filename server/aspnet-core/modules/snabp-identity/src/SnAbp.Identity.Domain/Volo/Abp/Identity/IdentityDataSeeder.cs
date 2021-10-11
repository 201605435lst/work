using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Identity
{
    public class IdentityDataSeeder : ITransientDependency, IIdentityDataSeeder
    {
        protected IGuidGenerator GuidGenerator { get; }
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected ILookupNormalizer LookupNormalizer { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentityRoleManager RoleManager { get; }

        protected IDataDictionaryRepository DataDictionaryRepository { get; }

        public IdentityDataSeeder(
            IGuidGenerator guidGenerator,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            ILookupNormalizer lookupNormalizer,
            IDataDictionaryRepository dataDictionaryRepository,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager)
        {
            GuidGenerator = guidGenerator;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            DataDictionaryRepository = dataDictionaryRepository;
            LookupNormalizer = lookupNormalizer;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        [UnitOfWork]
        public virtual async Task<IdentityDataSeedResult> SeedAsync(
            string adminEmail,
            string adminPassword,
            Guid? tenantId = null)
        {
            Check.NotNullOrWhiteSpace(adminEmail, nameof(adminEmail));
            Check.NotNullOrWhiteSpace(adminPassword, nameof(adminPassword));

            var result = new IdentityDataSeedResult();

            //"admin" user
            const string adminUserName = "admin";
            var adminUser = await UserRepository.FindByNormalizedUserNameAsync(
                LookupNormalizer.NormalizeName(adminUserName)
            );

            if (adminUser != null)
            {
                return result;
            }

            adminUser = new IdentityUser(
                GuidGenerator.Create(),
                adminUserName,
                adminEmail,
                tenantId
            )
            {
                Name = adminUserName
            };

            (await UserManager.CreateAsync(adminUser, adminPassword)).CheckErrors();
            result.CreatedAdminUser = true;

            //"admin" role
            const string adminRoleName = "admin";
            var adminRole = await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName(adminRoleName));
            if (adminRole == null)
            {
                // 逻辑修改，固定角色id Easten 修改
                adminRole = new IdentityRole(
                    Guid.Parse("5C146D09-2530-4545-8673-B518DB3F94FC"),
                    adminRoleName,
                    tenantId
                )
                {
                    IsStatic = true,
                    IsPublic = false
                };
                // 根据需求进行修改，角色名称添加角色的id  xxx@guid  为了解决授权问题  Easten 修改
                adminRole.ChangeName($"{adminRole.Name}@{adminRole.Id}");
                (await RoleManager.CreateAsync(adminRole)).CheckErrors();
                result.CreatedAdminRole = true;
            }

            (await UserManager.AddToRoleAsync(adminUser, adminRoleName)).CheckErrors();


            // 添加一个默认的角色
            var defaultRole = await RoleRepository.FindByNormalizedNameAsync("default");
            if (defaultRole == null)
            {
                // 逻辑修改，固定角色id Easten 修改
                defaultRole = new IdentityRole(
                    Guid.Parse("5C146D09-2530-4545-8673-B518DB3F94AA"),
                    "default",
                    tenantId
                )
                {
                    IsStatic = true,
                    IsPublic = false,
                    IsDefault =true
                };
                // 根据需求进行修改，角色名称添加角色的id  xxx@guid  为了解决授权问题  Easten 修改
                defaultRole.ChangeName($"{defaultRole.Name}@{defaultRole.Id}");
                (await RoleManager.CreateAsync(defaultRole)).CheckErrors();
                result.CreatedAdminRole = true;
            }

            (await UserManager.AddToRoleAsync(adminUser, "default")).CheckErrors();

            // 添加数字字典种子数据
            var dataDictionarys = IdentityDataDictionaryDataSeeder.GetDataDictionaryDataSeed();

            foreach (var dataDictionary in dataDictionarys)
            {
                dataDictionary.SetId(GuidGenerator.Create());
                await DataDictionaryRepository.InsertAsync(dataDictionary);
            }

            result.CreatedDataDictionary = true;

            return result;
        }
    }
}