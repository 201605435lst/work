using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Settings;
using SnAbp.Safe.Entities;

using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Safe.EntityFrameworkCore
{
    public static class SafeDbContextModelCreatingExtensions
    {
        public static void ConfigureSafe(
            this ModelBuilder builder,
            Action<SafeModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new SafeModelBuilderConfigurationOptions(
                SafeDbProperties.DbTablePrefix,
                SafeDbProperties.DbSchema
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

            builder.Entity<SafeProblemRecordRltFile>(b => b.ToTable(options.TablePrefix + "SafeProblemRecordRleFile", options.Schema));
            builder.Entity<SafeProblem>(b => b.ToTable(options.TablePrefix + "SafeProblem", options.Schema));
            builder.Entity<SafeProblemLibrary>(b => b.ToTable(options.TablePrefix + "SafeProblemLibrary", options.Schema));
            builder.Entity<SafeProblemLibraryRltScop>(b => b.ToTable(options.TablePrefix + "SafeProblemLibraryRltScop", options.Schema));
            builder.Entity<SafeProblemRecord>(b => b.ToTable(options.TablePrefix + "SafeProblemRecord", options.Schema));
            builder.Entity<SafeProblemRltCcUser>(b => b.ToTable(options.TablePrefix + "SafeProblemRltCcUser", options.Schema));
            builder.Entity<SafeProblemRltEquipment>(b => b.ToTable(options.TablePrefix + "SafeProblemRltEquipment", options.Schema));
            builder.Entity<SafeProblemRltFile>(b => b.ToTable(options.TablePrefix + "SafeProblemRltFile", options.Schema));
            builder.Entity<SafeSpeechVideo>(b => b.ToTable(options.TablePrefix + "SafeSpeechVideo", options.Schema));
            #region 依赖模块
            builder.Entity<DataDictionary>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + nameof(DataDictionary), SystemSettings.DbSchema);
            });
            builder.Entity<Organization>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(Organization), SystemSettings.DbSchema);
            });
            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + "Users", SystemSettings.DbSchema);
            });
            builder.Entity<File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });
            builder.Entity<Equipment>(b =>
            {
                b.ToTable(ResourceSettings.DbTablePrefix + nameof(Equipment), ResourceSettings.DbSchema);
            });
            #endregion

        }
    }
}