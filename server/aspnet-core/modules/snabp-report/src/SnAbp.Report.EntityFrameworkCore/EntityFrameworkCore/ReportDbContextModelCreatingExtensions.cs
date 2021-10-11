using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Project.Settings;
using SnAbp.Report.Entities;
using SnAbp.Report.Settings;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Report.EntityFrameworkCore
{
    public static class ReportDbContextModelCreatingExtensions
    {
        public static void ConfigureReport(
            this ModelBuilder builder,
            Action<ReportModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ReportModelBuilderConfigurationOptions(
                ReportDbProperties.DbTablePrefix,
                ReportDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */
            #region 依赖模块
            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + "Users", SnAbpIdentityDbProperties.DbSchema);
            });
            builder.Entity<Organization>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(Organization), SystemSettings.DbSchema);
            });
            builder.Entity<Project.Project>(b =>
            {
                b.ToTable(ProjectSettings.DbTablePrefix + nameof(Project), ProjectSettings.DbSchema);
            });
            builder.Entity<File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });
            #endregion


            #region 当前模块
          
            builder.Entity<Report>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Report), ReportSettings.DbSchema);
                b.ConfigureByConvention();
            });
            builder.Entity<ReportRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ReportRltFile), ReportSettings.DbSchema);
                b.ConfigureByConvention();
            });
            builder.Entity<ReportRltUser>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ReportRltUser), ReportSettings.DbSchema);
                b.ConfigureByConvention();
            });
            #endregion
        }
    }
}