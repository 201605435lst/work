using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Regulation.MongoDB
{
    [ConnectionStringName(RegulationDbProperties.ConnectionStringName)]
    public class RegulationMongoDbContext : AbpMongoDbContext, IRegulationMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureRegulation();
        }
    }
}