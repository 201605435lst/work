using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Quality.Entities;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Settings;
using Volo.Abp;

namespace SnAbp.Quality.EntityFrameworkCore
{
    public static class QualityDbContextModelCreatingExtensions
    {
        public static void ConfigureQuality(
            this ModelBuilder builder,
            Action<QualityModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new QualityModelBuilderConfigurationOptions(
                QualityDbProperties.DbTablePrefix,
                QualityDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<QualityProblemRecordRltFile>(b => b.ToTable(options.TablePrefix + "QualityProblemRecordRleFile", options.Schema));
            builder.Entity<QualityProblem>(b => b.ToTable(options.TablePrefix + "QualityProblem", options.Schema));
            builder.Entity<QualityProblemLibrary>(b => b.ToTable(options.TablePrefix + "QualityProblemLibrary", options.Schema));
            builder.Entity<QualityProblemLibraryRltScop>(b => b.ToTable(options.TablePrefix + "QualityProblemLibraryRltScop", options.Schema));
            builder.Entity<QualityProblemRecord>(b => b.ToTable(options.TablePrefix + "QualityProblemRecord", options.Schema));
            builder.Entity<QualityProblemRltCcUser>(b => b.ToTable(options.TablePrefix + "QualityProblemRltCcUser", options.Schema));
            builder.Entity<QualityProblemRltEquipment>(b => b.ToTable(options.TablePrefix + "QualityProblemRltEquipment", options.Schema));
            builder.Entity<QualityProblemRltFile>(b => b.ToTable(options.TablePrefix + "QualityProblemRltFile", options.Schema));
            builder.Entity<DataDictionary>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(DataDictionary), SystemSettings.DbSchema);
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
        }
    }
}