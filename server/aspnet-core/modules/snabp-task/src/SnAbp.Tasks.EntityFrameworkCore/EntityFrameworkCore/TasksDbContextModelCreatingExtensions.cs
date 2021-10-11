using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Project.Settings;
using SnAbp.Tasks.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Tasks.EntityFrameworkCore
{
    public static class TasksDbContextModelCreatingExtensions
    {
        public static void ConfigureTasks(
            this ModelBuilder builder,
            Action<TasksModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new TasksModelBuilderConfigurationOptions(
                TasksDbProperties.DbTablePrefix,
                TasksDbProperties.DbSchema
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

            builder.Entity<Project.Project>(b =>
            {
                b.ToTable(ProjectSettings.DbTablePrefix + nameof(Project), ProjectSettings.DbSchema);
            });

            #endregion


            #region 当前模块

            builder.Entity<Task>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Task), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<TaskRltMember>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(TaskRltMember), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<TaskRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(TaskRltFile), options.Schema);
                b.ConfigureByConvention();
            });

            #endregion
        }
    }
}