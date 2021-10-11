using System;

using Microsoft.EntityFrameworkCore;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Oa.Entities;

using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Oa.EntityFrameworkCore
{
    public static class OaDbContextModelCreatingExtensions
    {
        public static void ConfigureOa(
            this ModelBuilder builder,
            Action<OaModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OaModelBuilderConfigurationOptions(
                OaDbProperties.DbTablePrefix,
                OaDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                Indexes
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
            builder.Entity<DataDictionary>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + nameof(DataDictionary), SnAbpIdentityDbProperties.DbSchema);
            });
            builder.Entity<File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });
            #endregion


            #region 当前模块
            builder.Entity<DutySchedule>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(DutySchedule), options.Schema);
                b.ConfigureByConvention();
            });
            builder.Entity<DutyScheduleRltUser>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(DutyScheduleRltUser), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<Contract>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Contract), SnAbpIdentityDbProperties.DbSchema);
                b.ConfigureFullAudited();
            });
            builder.Entity<ContractRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(ContractRltFile), SnAbpIdentityDbProperties.DbSchema);
            });
            builder.Entity<Seal>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Seal), SnAbpIdentityDbProperties.DbSchema);
                b.ConfigureFullAudited();
            });
            builder.Entity<SealRltMember>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(SealRltMember), SnAbpIdentityDbProperties.DbSchema);
            });
            #endregion
        }
    }
}