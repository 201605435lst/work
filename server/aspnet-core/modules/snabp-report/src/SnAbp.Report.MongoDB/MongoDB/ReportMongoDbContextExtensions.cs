using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace SnAbp.Report.MongoDB
{
    public static class ReportMongoDbContextExtensions
    {
        public static void ConfigureReport(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ReportMongoModelBuilderConfigurationOptions(
                ReportDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}