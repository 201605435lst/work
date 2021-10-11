using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Report.MongoDB
{
    [ConnectionStringName(ReportDbProperties.ConnectionStringName)]
    public class ReportMongoDbContext : AbpMongoDbContext, IReportMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureReport();
        }
    }
}