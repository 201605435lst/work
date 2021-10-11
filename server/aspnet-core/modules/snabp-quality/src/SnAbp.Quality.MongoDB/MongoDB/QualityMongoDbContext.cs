using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Quality.MongoDB
{
    [ConnectionStringName(QualityDbProperties.ConnectionStringName)]
    public class QualityMongoDbContext : AbpMongoDbContext, IQualityMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureQuality();
        }
    }
}