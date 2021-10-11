using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Common.Entities;
//using SnAbp.Common.Eitities;
using Volo.Abp;

namespace SnAbp.Common.EntityFrameworkCore
{
    public static class CommonDbContextModelCreatingExtensions
    {
        public static void ConfigureCommon(
            this ModelBuilder builder,
            Action<CommonModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CommonModelBuilderConfigurationOptions(
                CommonDbProperties.DbTablePrefix,
                CommonDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);
                    
            builder.Entity<Area>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(Area), options.Schema);
            });
            builder.Entity<QRCode>(b =>
            {
                b.ToTable(options.TablePrefix + nameof(QRCode), options.Schema);
            });
        }
    }
}