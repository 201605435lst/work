using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using SnAbp.File.Entities;
using SnAbp.File.OssSdk;
using SnAbp.File.Repositories;
using SnAbp.Identity;

using Volo.Abp.Modularity; //using SnAbp.File.Repositories;

namespace SnAbp.File.EntityFrameworkCore
{
    [DependsOn(
        typeof(FileDomainModule),
        typeof(SnAbpIdentityDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class FileEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FileDbContext>(options =>
            {
                options.AddDefaultRepositories<IFileDbContext>(true);

                // 3.0.5�汾��Ҫ���Ӳִ�
                options.AddRepository<FileVersion, EfCoreRepository<IFileDbContext, FileVersion, Guid>>();
                options.AddRepository<Entities.File, EfCoreRepository<IFileDbContext, Entities.File, Guid>>();
                options.AddRepository<FileRltTag, EfCoreRepository<IFileDbContext, FileRltTag, Guid>>();
                options.AddRepository<FileRltShare, EfCoreRepository<IFileDbContext, FileRltShare, Guid>>();
                options.AddRepository<FileRltPermissions, EfCoreRepository<IFileDbContext, FileRltPermissions, Guid>>();
                options.AddRepository<Folder, EfCoreRepository<IFileDbContext, Folder, Guid>>();
                options.AddRepository<FolderRltTag, EfCoreRepository<IFileDbContext, FolderRltTag, Guid>>();
                options.AddRepository<FolderRltShare, EfCoreRepository<IFileDbContext, FolderRltShare, Guid>>();
                options.AddRepository<FolderRltPermissions, EfCoreRepository<IFileDbContext, FolderRltPermissions, Guid>>();
                options.AddRepository<Tag, EfCoreRepository<IFileDbContext, Tag, Guid>>();
                options.AddRepository<IdentityUserRltOrganization, EfCoreRepository<IFileDbContext, IdentityUserRltOrganization>>();
                options.AddRepository<Organization, EfCoreRepository<IFileDbContext, Organization>>();
                options.AddRepository<OssServer, EfCoreRepository<IFileDbContext, OssServer, Guid>>();
                //options.AddRepository<Organization, EfCoreRepository<Org, Tag, Guid>>();
                options.Services.AddTransient<ICustomRepository, CustomRepository>();


                options.Entity<Folder>(a => a.DefaultWithDetailsFunc = b => b
                    .Include(c => c.Permissions)
                    .Include(c => c.Tags).ThenInclude(c => c.Tag)
                    .Include(c => c.Files)
                    .Include(c => c.Files).ThenInclude(d => d.Versions)
                    .Include(c => c.Shares)
                    .Include(c => c.Folders)
                    .IgnoreQueryFilters()

                );
                options.Entity<Entities.File>(a => a.DefaultWithDetailsFunc = b => b
                    .Include(c => c.Tags).ThenInclude(c => c.Tag)
                    .Include(c => c.Permissions)
                    .Include(c => c.Versions).ThenInclude(c => c.Oss)
                    .Include(a => a.Shares)
                    .Include(a => a.Folder).ThenInclude(b => b.Shares)
                    .Include(a => a.Folder).ThenInclude(b => b.Permissions)
                );

                options.Entity<FolderRltTag>(a => a.DefaultWithDetailsFunc = b => b
                    .Include(c => c.Tag)
                    .Include(c => c.Folder).ThenInclude(c => c.Tags).ThenInclude(c => c.Tag)
                    .Include(c => c.Folder).ThenInclude(c => c.Shares)
                    .Include(c => c.Folder).ThenInclude(c => c.Permissions)
                );
                options.Entity<FileRltTag>(a => a.DefaultWithDetailsFunc = b => b
                    .Include(c => c.Tag)
                    .Include(c => c.File).ThenInclude(c => c.Tags).ThenInclude(c => c.Tag)
                    .Include(c => c.File).ThenInclude(c => c.Versions)
                    .Include(c => c.File).ThenInclude(c => c.Shares)
                    .Include(c => c.File).ThenInclude(c => c.Permissions)
                );
            });

            context.Services.AddSingleton<IOssRepository, OssRepository>();
        }
    }
}