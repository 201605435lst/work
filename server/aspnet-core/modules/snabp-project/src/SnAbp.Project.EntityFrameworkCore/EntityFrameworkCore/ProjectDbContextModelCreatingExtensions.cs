using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Project.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Project.EntityFrameworkCore
{
    public static class ProjectDbContextModelCreatingExtensions
    {
        public static void ConfigureProject(
            this ModelBuilder builder,
            Action<ProjectModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ProjectModelBuilderConfigurationOptions(
                ProjectDbProperties.DbTablePrefix,
                ProjectDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

          
            builder.Entity<Project>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Project), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<Unit>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Unit), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<ProjectRltContract>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProjectRltContract), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<ProjectRltMember>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProjectRltMember), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<ProjectRltUnit>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProjectRltUnit), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<ProjectRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ProjectRltFile), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<FileCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FileCategory), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<DossierRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(DossierRltFile), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<DossierCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(DossierCategory), options.Schema);
                b.ConfigureFullAudited();
                b.ConfigureByConvention();
            });
            builder.Entity<Dossier>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Dossier), options.Schema);
                b.ConfigureFullAudited();
                b.ConfigureByConvention();

            });
            builder.Entity<ArchivesCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ArchivesCategory), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });
            builder.Entity<BooksClassification>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(BooksClassification), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<Archives>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Archives), options.Schema);
                b.ConfigureByConvention();
                b.ConfigureFullAudited();
            });
            builder.Entity<FileCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FileCategory), options.Schema);
                b.ConfigureByConvention();
            });
        }
    }
}