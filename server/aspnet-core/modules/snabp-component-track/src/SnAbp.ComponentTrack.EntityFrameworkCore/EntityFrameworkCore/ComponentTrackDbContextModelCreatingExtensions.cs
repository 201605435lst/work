using System;
using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.Settings;
using SnAbp.File.Settings;
using SnAbp.Identity;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Settings;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Settings;
using Volo.Abp;
using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.ComponentTrack.EntityFrameworkCore
{
    public static class ComponentTrackDbContextModelCreatingExtensions
    {
        public static void ConfigureComponentTrack(
            this ModelBuilder builder,
            Action<ComponentTrackModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ComponentTrackModelBuilderConfigurationOptions(
                ComponentTrackDbProperties.DbTablePrefix,
                ComponentTrackDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

        }
    }
}
