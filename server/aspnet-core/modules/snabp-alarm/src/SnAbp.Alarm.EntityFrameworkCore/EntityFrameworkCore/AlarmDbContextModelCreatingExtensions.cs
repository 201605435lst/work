using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Alarm.Entities;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Alarm.EntityFrameworkCore
{
    public static class AlarmDbContextModelCreatingExtensions
    {
        public static void ConfigureAlarm(
            this ModelBuilder builder,
            Action<AlarmModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new AlarmModelBuilderConfigurationOptions(
                AlarmDbProperties.DbTablePrefix,
                AlarmDbProperties.DbSchema
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


            builder.Entity<Entities.Alarm>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Entities.Alarm), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<AlarmConfig>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(AlarmConfig), options.Schema);
                b.ConfigureByConvention();
            });

            builder.Entity<AlarmEquipmentIdBind>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(AlarmEquipmentIdBind), options.Schema);
                b.ConfigureByConvention();
            });
        }
    }
}