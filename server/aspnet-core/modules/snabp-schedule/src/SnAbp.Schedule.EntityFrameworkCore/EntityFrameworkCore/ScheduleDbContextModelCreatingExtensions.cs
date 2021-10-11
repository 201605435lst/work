using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Schedule.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Schedule.EntityFrameworkCore
{
    public static class ScheduleDbContextModelCreatingExtensions
    {
        public static void ConfigureSchedule(
            this ModelBuilder builder,
            Action<ScheduleModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ScheduleModelBuilderConfigurationOptions(
                ScheduleDbProperties.DbTablePrefix,
                ScheduleDbProperties.DbSchema
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

            #region 当前模块

            builder.Entity<Schedule>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(Schedule), options.Schema);
                builder.ConfigureByConvention();
            });
            builder.Entity<ScheduleRltSchedule>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ScheduleRltSchedule), options.Schema);
                builder.ConfigureByConvention();
                //builder.HasOne(x => x.Schedule).WithMany();
                //builder.HasOne(x => x.FrontSchedule).WithMany();
            });
            builder.Entity<ScheduleFlowInfo>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ScheduleFlowInfo), options.Schema);
                builder.ConfigureByConvention();
            });
            builder.Entity<ScheduleFlowTemplate>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ScheduleFlowTemplate), options.Schema);
                builder.ConfigureByConvention();
            });
            builder.Entity<ScheduleRltProjectItem>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ScheduleRltProjectItem), options.Schema);
                builder.ConfigureByConvention();
            });
            builder.Entity<ScheduleRltEquipment>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ScheduleRltEquipment), options.Schema);
                builder.ConfigureByConvention();
            });

            builder.Entity<Approval>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(Approval), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<ApprovalRltFile>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ApprovalRltFile), options.Schema);
            });
            builder.Entity<ApprovalRltMaterial>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ApprovalRltMaterial), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<ApprovalRltMember>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(ApprovalRltMember), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<Diary>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(Diary), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<DiaryRltMaterial>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(DiaryRltMaterial), options.Schema);
                builder.ConfigureFullAudited();
            });
            builder.Entity<DiaryRltBuilder>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(DiaryRltBuilder), options.Schema);
                builder.ConfigureByConvention();
            });
            builder.Entity<DiaryRltFile>(builder =>
            {
                builder.ToTable(options.TablePrefix + nameof(DiaryRltFile), options.Schema);
                builder.ConfigureByConvention();
            });


            #endregion
            #region  依赖模块
            builder.Entity<SnAbp.File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(SnAbp.File.Entities.File), FileSettings.DbSchema);
            });
            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + "Users", SnAbpIdentityDbProperties.DbSchema);
            });
            builder.Entity<DataDictionary>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + "DataDictionary", SnAbpIdentityDbProperties.DbSchema);
            });
            #endregion   
        }
    }
}