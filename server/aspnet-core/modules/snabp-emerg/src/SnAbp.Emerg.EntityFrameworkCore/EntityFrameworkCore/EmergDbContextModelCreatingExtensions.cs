using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Settings;
using SnAbp.Emerg.Entities;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Settings;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Settings;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Emerg.EntityFrameworkCore
{
    public static class EmergDbContextModelCreatingExtensions
    {
        public static void ConfigureEmerg(
            this ModelBuilder builder,
            Action<EmergModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new EmergModelBuilderConfigurationOptions(
                EmergDbProperties.DbTablePrefix,
                EmergDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<File.Entities.File>(b =>
            {
                b.ToTable(FileSettings.DbTablePrefix + nameof(File), FileSettings.DbSchema);
            });

            builder.Entity<Equipment>(b =>
            {
                b.ToTable(ResourceSettings.DbTablePrefix + nameof(Equipment), ResourceSettings.DbSchema);
            });

            builder.Entity<EquipmentGroup>(b =>
            {
                b.ToTable(ResourceSettings.DbTablePrefix + nameof(EquipmentGroup), ResourceSettings.DbSchema);
            });

            builder.Entity<Station>(b =>
            {
                b.ToTable(BasicSettings.DbTablePrefix + nameof(Station), BasicSettings.DbSchema);
            });

            builder.Entity<ComponentCategory>(b =>
            {
                b.ToTable(StdBasicSettings.DbTablePrefix + nameof(ComponentCategory), StdBasicSettings.DbSchema);
            });

            builder.Entity<DataDictionary>(b =>
            {
                b.ToTable(SystemSettings.DbTablePrefix + nameof(DataDictionary), SystemSettings.DbSchema);
            });

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable(SnAbpIdentityDbProperties.DbTablePrefix + "Users", SnAbpIdentityDbProperties.DbSchema);
            });

            //
            builder.Entity<EmergPlan>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlan), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<EmergPlanRecord>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlanRecord), options.Schema);
                b.ConfigureFullAudited();
            });

            builder.Entity<EmergPlanProcessRecord>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlanProcessRecord), options.Schema);
            });

            builder.Entity<EmergPlanRecordRltComponentCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlanRecordRltComponentCategory), options.Schema);
            });

            builder.Entity<EmergPlanRecordRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlanRecordRltFile), options.Schema);
            });

            builder.Entity<EmergPlanRecordRltMember>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlanRecordRltMember), options.Schema);
            });

            builder.Entity<FaultRltComponentCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FaultRltComponentCategory), options.Schema);
            });

            builder.Entity<EmergPlanRltComponentCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlanRltComponentCategory), options.Schema);
            });

            builder.Entity<EmergPlanRltFile>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(EmergPlanRltFile), options.Schema);
            });
            builder.Entity<Fault>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Fault), options.Schema);
                b.ConfigureFullAudited();
            });
            builder.Entity<FaultRltComponentCategory>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FaultRltComponentCategory), options.Schema);
            });
            builder.Entity<FaultRltEquipment>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(FaultRltEquipment), options.Schema);
            });
        }
    }
}