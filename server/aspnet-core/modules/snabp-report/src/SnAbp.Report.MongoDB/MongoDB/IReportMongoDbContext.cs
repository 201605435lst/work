using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Report.MongoDB
{
    [ConnectionStringName(ReportDbProperties.ConnectionStringName)]
    public interface IReportMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
