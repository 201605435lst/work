using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore.Modeling;
using SnAbp.File.Entities;
using SnAbp.Identity;
using Volo.Abp;

namespace SnAbp.File.EntityFrameworkCore
{
    public static class FileDbContextModelCreatingExtensions
    {
        public static void ConfigureFile(
            this ModelBuilder builder,
            Action<FileModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new FileModelBuilderConfigurationOptions(
                FileDbProperties.DbTablePrefix,
                FileDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Organization>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(Organization), SystemSettings.DbSchema);
                b.ConfigureByConvention();
            });

            builder.Entity<Entities.File>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(File), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });
            builder.Entity<FileRltPermissions>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FileRltPermissions), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<FileRltShare>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FileRltShare), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<FileRltTag>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FileRltTag), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<FileVersion>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FileVersion), options.Schema);
                b.ConfigureByConvention();
            });


            builder.Entity<Folder>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Folder), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });
            builder.Entity<FolderRltPermissions>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FolderRltPermissions), options.Schema);
                b.ConfigureByConvention();

            });
            builder.Entity<FolderRltShare>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FolderRltShare), options.Schema);
                b.ConfigureByConvention();

            });
            builder.Entity<FolderRltTag>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FolderRltTag), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<Tag>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Tag), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<OssServer>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(OssServer), options.Schema);
                b.ConfigureByConvention();
            });
        }
    }
}